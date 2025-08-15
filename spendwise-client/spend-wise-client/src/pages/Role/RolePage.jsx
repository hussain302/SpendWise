import React from "react";
import RoleModel from "./RoleModel";

const RolePage = () => {
  return (
    <>
      <section>
        <div>
          <h2 className="mb-3 container mt-4">Manage System Roles</h2>
          <RoleModel />
        </div>
      </section>
    </>
  );
};

export default RolePage;
