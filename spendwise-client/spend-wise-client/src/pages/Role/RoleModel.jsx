import React, { useState, useEffect } from "react";
import ActionButtons from "../../components/ui/ActionButtons";
import API_ROUTES from "../../utils/ApiRoutes";
import useApiHelper from "../../hooks/useApiHelper";
import ToastMessage from "../../components/ui/ToastMessage";
import API_HELPER from "../../utils/ApiRequestHelper";

const RoleModel = () => {
  const [modalType, setModalType] = useState("");
  const [selectedRole, setSelectedRole] = useState(null);
  const [roles, setRoles] = useState([]);
  const [toast, setToast] = useState({ show: false, type: "", message: "" });
  const [role, setRole] = useState({
    id: API_HELPER.EMPTY_GUID,
    name: "",
    description: "",
  });

  const apiHelper = useApiHelper();
  useEffect(() => {
    fetchRoles();
  }, []);

  //Handle Input Changes of Role
  const handleChange = (e) => {
    const { name, value } = e.target;
    setRole((prev) => ({ ...prev, [name]: value }));
  };

  //Update Role
  const handleUpdateRole = async (e) => {
    e.preventDefault();
    try {
      const data = await apiHelper.put(API_ROUTES.ROLES.UPDATE, role);

      if (!data.isSuccess) {
        setToast({
          show: true,
          type: "error",
          message: data.errors?.[0] || "Role update unsuccessful.",
        });
        return;
      }

      setToast({
        show: true,
        type: "success",
        message: data.successes?.[0] || "Role updated successfully.",
      });

      fetchRoles();

      const modalElement = document.getElementById("roleModal");
      const modal = window.bootstrap.Modal.getInstance(modalElement);

      if (modal) modal.hide();

      setRole({
        id: API_HELPER.EMPTY_GUID,
        name: "",
        description: "",
      });
    } catch (error) {
      setToast({
        show: true,
        type: "error",
        message: error.message,
      });
    }
  };

  //Delete Role
  const handleDeleteRole = async (e, id) => {
    e.preventDefault();
    try {
      const data = await apiHelper.delete(API_ROUTES.ROLES.DELETE(id));

      if (!data.isSuccess) {
        setToast({
          show: true,
          type: "error",
          message: data.errors?.[0] || "Role deletion unsuccessful.",
        });
        return;
      }

      setToast({
        show: true,
        type: "success",
        message: data.successes?.[0] || "Role deleted successfully.",
      });

      fetchRoles();

      const modalElement = document.getElementById("roleModal");
      const modal = window.bootstrap.Modal.getInstance(modalElement);

      if (modal) modal.hide();

      // setRole({
      //   id: API_HELPER.EMPTY_GUID,
      //   name: "",
      //   description: "",
      // });
    } catch (error) {
      setToast({
        show: true,
        type: "error",
        message: error.message,
      });
    }
  };

  //Create Role
  const handleCreateRole = async (e) => {
    e.preventDefault();
    try {
      const data = await apiHelper.post(API_ROUTES.ROLES.CREATE, role);

      if (!data.isSuccess) {
        setToast({
          show: true,
          type: "error",
          message: data.errors?.[0] || "Role creation unsuccessful.",
        });
        return;
      }

      setToast({
        show: true,
        type: "success",
        message: data.successes?.[0] || "Role created successfully.",
      });

      fetchRoles();

      const modalElement = document.getElementById("roleModal");
      const modal = window.bootstrap.Modal.getInstance(modalElement);

      if (modal) modal.hide();

      setRole({
        id: API_HELPER.EMPTY_GUID,
        name: "",
        description: "",
      });
    } catch (error) {
      setToast({
        show: true,
        type: "error",
        message: error.message,
      });
    }
  };

  //Get all Roles
  const fetchRoles = async () => {
    try {
      const data = await apiHelper.get(API_ROUTES.ROLES.LIST);
      setRoles(data.value);
    } catch (error) {
      setToast({
        show: true,
        type: "error",
        message: error.message,
      });
    }
  };

  //Genaric model for all types of requests (GET, POST, PUT, DELETE)
  const handleShowModal = (type, role) => {
    setModalType(type);

    if (type === "edit" && role) {
      setRole({ id: role.id, name: role.name, description: role.description });
    } else {
      setRole({ id: API_HELPER.EMPTY_GUID, name: "", description: "" });
    }

    setSelectedRole(role);
    const modal = new window.bootstrap.Modal(
      document.getElementById("roleModal")
    );
    modal.show();
  };

  return (
    <div className="container mt-4">
      {toast && (
        <ToastMessage
          type={toast.type}
          message={toast.message}
          show={toast.show}
          onClose={() => setToast({ ...toast, show: false })}
        />
      )}
      <h2 className="d-flex justify-content-between align-items-center">
        Role Management
        <a
          className="btn btn-outline-success d-flex align-items-center gap-2"
          onClick={() => handleShowModal("add")}
        >
          <i className="bi bi-plus-lg"></i> Create
          <svg
            xmlns="http://www.w3.org/2000/svg"
            width="16"
            height="16"
            fill="currentColor"
            className="bi bi-plus-lg"
            viewBox="0 0 16 16"
          >
            <path
              fillRule="evenodd"
              d="M8 2a.5.5 0 0 1 .5.5v5h5a.5.5 0 0 1 0 1h-5v5a.5.5 0 0 1-1 0v-5h-5a.5.5 0 0 1 0-1h5v-5A.5.5 0 0 1 8 2"
            />
          </svg>
        </a>
      </h2>

      <table className="table table-hover">
        <thead>
          <tr className="table-success">
            <th>#</th>
            <th>Name</th>
            <th>Description</th>
            <th>Action</th>
          </tr>
        </thead>
        <tbody>
          {roles.map((role, index) => (
            <tr key={role.id}>
              <th scope="row">{index + 1}</th>
              <td>{role.name}</td>
              <td>{role.description}</td>
              <td>
                <ActionButtons
                  editId={`edit-btn-${role.id}`}
                  deleteId={`delete-btn-${role.id}`}
                  detailId={`detail-btn-${role.id}`}
                  onEdit={() => handleShowModal("edit", role)}
                  onDelete={() => handleShowModal("delete", role)}
                  onDetail={() => handleShowModal("detail", role)}
                  showEdit={true}
                  showDelete={true}
                  showDetail={true}
                  editText=""
                  deleteText=""
                  detailText=""
                />
              </td>
            </tr>
          ))}
        </tbody>
      </table>

      {/* Bootstrap Modal */}
      <div
        className="modal fade"
        id="roleModal"
        tabIndex="-1"
        aria-hidden="true"
      >
        <div className="modal-dialog" role="document">
          <div className="modal-content">
            <div className="modal-header">
              <h5 className="modal-title">
                {modalType === "edit"
                  ? "Edit Role"
                  : modalType === "delete"
                  ? "Delete Confirmation"
                  : "Role Details"}
              </h5>
              <button
                type="button"
                className="btn-close"
                data-bs-dismiss="modal"
              ></button>
            </div>
            <div className="modal-body">
             
              {modalType === "edit" && selectedRole && (
                <form>
                  <div className="mb-3">
                    <input
                      type="hidden"
                      className="form-control"
                      defaultValue={selectedRole.id}
                    />

                    <label className="form-label">Role Name</label>
                    <input
                      type="text"
                      className="form-control"
                      name="name"
                      onChange={handleChange}
                      //defaultValue={selectedRole.name}
                      value={role.name}
                    />
                  </div>
                  <div className="mb-3">
                    <label className="form-label">Description</label>
                    <textarea
                      className="form-control"
                      rows="3"
                      name="description"
                      onChange={handleChange}
                      // defaultValue={selectedRole.description}
                      value={role.description}
                    ></textarea>
                  </div>
                </form>
              )}

              {modalType === "add" && (
                <form>
                  <div className="mb-3">
                    <label className="form-label">Role Name</label>
                    <input
                      name="name"
                      type="text"
                      className="form-control"
                      value={role.name}
                      onChange={handleChange}
                      required
                    />
                  </div>
                  <div className="mb-3">
                    <label className="form-label">Description</label>
                    <textarea
                      className="form-control"
                      name="description"
                      rows="3"
                      value={role.description}
                      onChange={handleChange}
                      required
                    ></textarea>
                  </div>
                </form>
              )}

              {modalType === "delete" && selectedRole && (
                <span>
                  Are you sure you want to delete the role "{selectedRole.name}
                  "?
                </span>
              )}

              {modalType === "detail" && selectedRole && (
                <form>
                <div className="mb-3">
                  <label className="form-label">Role Name</label>
                  <input
                    name="name"
                    type="text"
                    className="form-control"
                    value={selectedRole.name}
                    disabled
                  />
                </div>
                <div className="mb-3">
                  <label className="form-label">Description</label>
                  <textarea
                    className="form-control"
                    name="description"
                    rows="3"
                    value={selectedRole.description}
                    disabled
                  ></textarea>
                </div>
              </form>
              )}

            </div>
            <div className="mb-4 d-flex justify-content-center">
             
              {modalType === "add" && (
                <button
                  type="submit"
                  onClick={handleCreateRole}
                  className="btn btn-outline-success"
                >
                  Create{" "}
                  <svg
                    xmlns="http://www.w3.org/2000/svg"
                    width="16"
                    height="16"
                    fill="currentColor"
                    className="bi bi-plus-lg"
                    viewBox="0 0 16 16"
                  >
                    <path
                      fillRule="evenodd"
                      d="M8 2a.5.5 0 0 1 .5.5v5h5a.5.5 0 0 1 0 1h-5v5a.5.5 0 0 1-1 0v-5h-5a.5.5 0 0 1 0-1h5v-5A.5.5 0 0 1 8 2"
                    />
                  </svg>
                </button>
              )}

              {modalType === "edit" && selectedRole && (
                <button type="button" onClick={handleUpdateRole} className="btn btn-outline-warning">
                  Update{" "}
                  <svg
                    xmlns="http://www.w3.org/2000/svg"
                    width="16"
                    height="16"
                    fill="currentColor"
                    className="bi bi-pencil-square"
                    viewBox="0 0 16 16"
                  >
                    <path d="M15.502 1.94a.5.5 0 0 1 0 .706L14.459 3.69l-2-2L13.502.646a.5.5 0 0 1 .707 0l1.293 1.293zm-1.75 2.456-2-2L4.939 9.21a.5.5 0 0 0-.121.196l-.805 2.414a.25.25 0 0 0 .316.316l2.414-.805a.5.5 0 0 0 .196-.12l6.813-6.814z" />
                    <path
                      fillRule="evenodd"
                      d="M1 13.5A1.5 1.5 0 0 0 2.5 15h11a1.5 1.5 0 0 0 1.5-1.5v-6a.5.5 0 0 0-1 0v6a.5.5 0 0 1-.5.5h-11a.5.5 0 0 1-.5-.5v-11a.5.5 0 0 1 .5-.5H9a.5.5 0 0 0 0-1H2.5A1.5 1.5 0 0 0 1 2.5z"
                    />
                  </svg>
                </button>
              )}

              {modalType === "delete" && selectedRole && (
                <button
                  type="button"
                  onClick={(e) => handleDeleteRole(e, selectedRole.id)}
                  className="btn btn-outline-danger"
                >
                  Delete{" "}
                  <svg
                    xmlns="http://www.w3.org/2000/svg"
                    width="16"
                    height="16"
                    fill="currentColor"
                    className="bi bi-trash3-fill"
                    viewBox="0 0 16 16"
                  >
                    <path d="M11 1.5v1h3.5a.5.5 0 0 1 0 1h-.538l-.853 10.66A2 2 0 0 1 11.115 16h-6.23a2 2 0 0 1-1.994-1.84L2.038 3.5H1.5a.5.5 0 0 1 0-1H5v-1A1.5 1.5 0 0 1 6.5 0h3A1.5 1.5 0 0 1 11 1.5m-5 0v1h4v-1a.5.5 0 0 0-.5-.5h-3a.5.5 0 0 0-.5.5M4.5 5.029l.5 8.5a.5.5 0 1 0 .998-.06l-.5-8.5a.5.5 0 1 0-.998.06m6.53-.528a.5.5 0 0 0-.528.47l-.5 8.5a.5.5 0 0 0 .998.058l.5-8.5a.5.5 0 0 0-.47-.528M8 4.5a.5.5 0 0 0-.5.5v8.5a.5.5 0 0 0 1 0V5a.5.5 0 0 0-.5-.5" />
                  </svg>
                </button>
              )}

              <button
                type="button"
                className="btn btn-outline-primary mx-1"
                data-bs-dismiss="modal"
              >
                Close{" "}
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  width="16"
                  height="16"
                  fill="currentColor"
                  className="bi bi-x-circle"
                  viewBox="0 0 16 16"
                >
                  <path d="M8 15A7 7 0 1 1 8 1a7 7 0 0 1 0 14m0 1A8 8 0 1 0 8 0a8 8 0 0 0 0 16" />
                  <path d="M4.646 4.646a.5.5 0 0 1 .708 0L8 7.293l2.646-2.647a.5.5 0 0 1 .708.708L8.707 8l2.647 2.646a.5.5 0 0 1-.708.708L8 8.707l-2.646 2.647a.5.5 0 0 1-.708-.708L7.293 8 4.646 5.354a.5.5 0 0 1 0-.708" />
                </svg>
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default RoleModel;
