import React from "react";
import { Link } from "react-router-dom";

const NotFound = ({ page }) => {
  return (
    <div className="d-flex flex-column align-items-center justify-content-center vh-100 text-center">
      <h1 className="display-1 fw-bold text-danger">404</h1>
      <h2 className="fw-semibold text-dark">Oops! Page Not Found</h2>
      <p className="lead text-muted">
        The page you are looking for might have been removed, had its name changed, or is temporarily unavailable.
      </p>
      {/* <img
        src="https://source.unsplash.com/500x300/?error,404"
        alt="404 Not Found"
        className="mb-4 rounded shadow"
      /> */}
      <Link to={page || "/"} className="btn btn-primary">
        Go Back Home
      </Link>
    </div>
  );
};

export default NotFound;
