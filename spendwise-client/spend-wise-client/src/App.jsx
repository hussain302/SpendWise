import "./App.css";
import NavbarAdmin from "./layouts/NavbarAdmin";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import AdminHome from "./pages/Home/AdminHome";
import Contact from "./pages/Contact/Contact";
import About from "./pages/About/About";
import RolePage from "./pages/Role/RolePage";
import NotFound from "./components/ui/NotFound";

function App() {

  return (
    <>
      <header>
        {/* <Navbar /> */}
        <NavbarAdmin />
        <div className="container-fluid mt-4">
        <Routes>
        <Route path="*" element={<NotFound page="/admin-home" />} />
        <Route path="/admin-home" element={<AdminHome />} />
          <Route path="/about" element={<About />} />
          <Route path="/contact" element={<Contact />} />
          
          <Route path="/role" element={<RolePage />} />
          {/* <Route path="/user" element={<RolePage />} /> */}
        </Routes>
      </div>
      </header>
    </>
  );
}

export default App;
