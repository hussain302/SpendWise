import React from "react";

const AdminHome = () => {
  return (
    <div className="container mt-5">
      <h2 className="text-center fw-bold mb-4">Welcome to Spendwise Admin Panel</h2>
      
      <div className="row">
        {/* System Management Section */}
        <div className="col-md-4">
          <div className="card shadow-sm border-0">
            <div className="card-body text-center">
              <h5 className="card-title fw-semibold">System Management</h5>
              <p className="card-text text-muted">Manage roles, users, and organizations.</p>
              <a href="/role" className="btn btn-primary btn-sm me-2">Roles</a>
              <a href="/user" className="btn btn-primary btn-sm me-2">Users</a>
              <a href="/org" className="btn btn-primary btn-sm">Organizations</a>
            </div>
          </div>
        </div>

        {/* User Profile Section */}
        <div className="col-md-4">
          <div className="card shadow-sm border-0">
            <div className="card-body text-center">
              <h5 className="card-title fw-semibold">Admin Profile</h5>
              <p className="card-text text-muted">View and manage your admin settings.</p>
              <a href="#" className="btn btn-secondary btn-sm">Profile</a>
              <a href="#" className="btn btn-secondary btn-sm ms-2">Settings</a>
            </div>
          </div>
        </div>

        {/* Reports & Logs */}
        <div className="col-md-4">
          <div className="card shadow-sm border-0">
            <div className="card-body text-center">
              <h5 className="card-title fw-semibold">Reports & Logs</h5>
              <p className="card-text text-muted">Analyze logs and generate reports.</p>
              <a href="#" className="btn btn-success btn-sm">View Logs</a>
              <a href="#" className="btn btn-success btn-sm ms-2">Generate Reports</a>
            </div>
          </div>
        </div>
      </div>

      {/* Quick Actions Section */}
      <div className="row mt-4">
        <div className="col-md-12 text-center">
          <h5 className="fw-bold">Quick Actions</h5>
          <div className="d-flex justify-content-center gap-3">
            <a href="/user" className="btn btn-outline-primary">Manage Users</a>
            <a href="/role" className="btn btn-outline-warning">Manage Roles</a>
            <a href="#" className="btn btn-outline-danger">System Logs</a>
          </div>
        </div>
      </div>
    </div>
  );
};

export default AdminHome;
