import React from 'react';

const NavbarAdmin = () => {
  return (
    <nav className="navbar navbar-expand-lg navbar-light bg-light shadow-sm">
      <div className="container">
        {/* Brand Logo */}
        <a className="navbar-brand fw-bold" href="/admin-home">Spendwise</a>

        {/* Mobile Toggle Button */}
        <button 
          className="navbar-toggler" 
          type="button" 
          data-bs-toggle="collapse" 
          data-bs-target="#navbarNav"
          aria-controls="navbarNav"
          aria-expanded="false"
          aria-label="Toggle navigation"
        >
          <span className="navbar-toggler-icon"></span>
        </button>

        {/* Navbar Links */}
        <div className="collapse navbar-collapse" id="navbarNav">
          <ul className="navbar-nav me-auto">
            <li className="nav-item">
              <a className="nav-link active fw-semibold" href="/admin-home">Home</a>
            </li>          
            <div className="dropdown">
              <a className="nav-link dropdown-toggle" href="#" id="userDropdown" role="button" data-bs-toggle="dropdown" aria-expanded="false">
                System Management
              </a>
              <ul className="dropdown-menu dropdown-menu-end" aria-labelledby="userDropdown">
                <li><a className="dropdown-item" href="/role">Roles</a></li>
                <li><a className="dropdown-item" href="/user">Users</a></li>
                <li><a className="dropdown-item" href="#">Organizations</a></li>
              </ul>
            </div>
          </ul>

          {/* Right Side - Profile & Search */}
          <div className="d-flex align-items-center gap-3">
            {/* User Dropdown */}
            <div className="dropdown">
              <a className="nav-link dropdown-toggle fw-semibold" href="#" id="userDropdown" role="button" data-bs-toggle="dropdown" aria-expanded="false">
                M. Hussain (Admin)
              </a>
              <ul className="dropdown-menu dropdown-menu-start" aria-labelledby="userDropdown">
                <li><a className="dropdown-item" href="#">Profile</a></li>
                <li><a className="dropdown-item" href="#">Teams</a></li>
                <li><a className="dropdown-item" href="#">Settings</a></li>
                <li><hr className="dropdown-divider" /></li>
                <li><a className="dropdown-item text-danger" href="#">Logout</a></li>
              </ul>
            </div>

            {/* Search Box */}
            <form className="d-flex">
              <input className="form-control me-2" type="search" placeholder="Search..." aria-label="Search" />
              <button className="btn btn-outline-success" type="submit">Search</button>
            </form>
          </div>
        </div>
      </div>
    </nav>
  );
}

export default NavbarAdmin;
