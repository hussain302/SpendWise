import React, { useEffect, useState } from "react";

const ToastMessage = ({ type, message, show, onClose }) => {
  const [visible, setVisible] = useState(show);

  useEffect(() => {
    setVisible(show); // Update state when `show` prop changes
    if (show) {
      const timer = setTimeout(() => {
        setVisible(false);
        onClose();
      }, 4000);

      return () => clearTimeout(timer);
    }
  }, [show, onClose]);

  const toastClass = {
    success: "bg-success text-white",
    error: "bg-danger text-white",
    warning: "bg-warning text-dark",
    info: "bg-primary text-white",
  };

  if (!visible) return null; // Completely remove toast when not visible

  return (
    <div
      className={`toast show position-fixed top-0 end-0 m-3 ${toastClass[type]}`}
      role="alert"
      aria-live="assertive"
      aria-atomic="true"
      style={{ zIndex: 1050 }}
    >
      <div className="d-flex">
        <div className="toast-body">{message}</div>
        <button
          type="button"
          className="btn-close me-2 m-auto"
          aria-label="Close"
          onClick={() => {
            setVisible(false);
            onClose();
          }}
        ></button>
      </div>
    </div>
  );
};

export default ToastMessage;
