import axios from "axios";
import API_ROUTES from "../utils/ApiRoutes";
import { useToast } from "../context/ToastContext";

const BASE_URL = API_ROUTES.BASE_URL;

const useApiHelper = () => {
  const { showToast } = useToast();

  const handleError = (error) => {
    let message = "Something went wrong!";
    let status = error.response ? error.response.status : null;

    if (error.response) {
      switch (status) {
        case 400:
          message = "Bad request. Please check your input.";
          break;
        case 401:
          message = "Unauthorized. Please log in.";
          break;
        case 403:
          message = "Forbidden. You don't have permission.";
          break;
        case 404:
          message = "No rocords found.";
          break;
        case 429:
          message =
            "The user has sent too many requests in a given amount of time.";
          break;
        case 500:
          message = "Internal server error. Try again later.";
          break;
        default:
          message =
            error.response.data?.message || "Unexpected error occurred.";
      }
    } else if (error.request) {
      message = "No response from server. Check your network.";
    }

    console.error("API Error:", message);
    showToast("error", message);
    throw new Error(message);
  };

  const request = async (method, url, data = null, params = {}) => {
    try {
      const response = await axios({
        method,
        url: `${BASE_URL}${url}`,
        data,
        params,
      });
  
      return response.data;
    } catch (error) {
      
      const status = error.response?.status; // Use optional chaining
      const data = error.response?.data; 
      
      if (data?.isSuccess === false) {
        if (data.validationErrors?.Description[0].length > 0) {
          showToast("error", data.validationErrors?.Description[0]);
          throw new Error(data.validationErrors?.Description[0]);
        }

        showToast("error", data.errors?.[0] || "Operation failed.");
        throw new Error(data.errors?.[0] || "Operation failed.");
      }

      handleError(error);
    }
  };

  return {
    get: (url, params) => request("GET", url, null, params),
    post: (url, data) => request("POST", url, data),
    put: (url, data) => request("PUT", url, data),
    delete: (url) => request("DELETE", url),
  };
};

export default useApiHelper;
