import axios from "axios";

const API = "http://localhost:5202/api/Audit";

const token = () => localStorage.getItem("token");

export const getAuditLogs = async () => {
  const response = await axios.get(API, {
    headers: {
      Authorization: `Bearer ${token()}`
    }
  });

  return response.data;
};