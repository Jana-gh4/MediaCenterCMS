import { Routes, Route } from "react-router-dom";

import Login from "./pages/Login/Login";
import Home from "./pages/Home/Home";
import News from "./pages/News/News";
import Images from "./pages/Images/Images";
import Videos from "./pages/Videos/Videos";
import Approvals from "./pages/Approvals/Approvals";
import AuditLogs from "./pages/AuditLogs/AuditLogs";

function App() {
  return (
    <Routes>
      <Route path="/" element={<Login />} />
      <Route path="/home" element={<Home />} />
      <Route path="/news" element={<News />} />
      <Route path="/images" element={<Images />} />
      <Route path="/videos" element={<Videos />} />
      <Route path="/approvals" element={<Approvals />} />
      <Route path="/auditlogs" element={<AuditLogs />} />
    </Routes>
  );
}

export default App;