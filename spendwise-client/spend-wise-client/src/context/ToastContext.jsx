import React, { createContext, useContext, useState } from "react";
import ToastMessage from "../components/ui/ToastMessage";

const ToastContext = createContext();

export const ToastProvider = ({ children }) => {
  const [toast, setToast] = useState({ show: false, type: "", message: "" });

  const showToast = (type, message) => {
    setToast({ show: true, type, message });
    setTimeout(() => {
      setToast({ show: false, type: "", message: "" });
    }, 4000); 
  };

  return (
    <ToastContext.Provider value={{ showToast }}>
      {children}
      {toast.show && <ToastMessage type={toast.type} message={toast.message} />}
    </ToastContext.Provider>
  );
};

// Custom hook for using the toast
export const useToast = () => useContext(ToastContext);
