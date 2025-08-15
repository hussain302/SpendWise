const API_ROUTES = {
    BASE_URL : "http://localhost:5001/api",
    ROLES: {
      LIST: "/roles/GetAll",
      GET: (id) => `/roles/GetById?id=${id}`,
      CREATE: "/roles/Add",
      UPDATE: `/roles/Edit`,
      DELETE: (id) => `/roles/Delete?id=${id}`,
    }
  };
  
  export default API_ROUTES;
  