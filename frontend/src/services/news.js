import axios from "axios";

const API = "http://localhost:5202/api/News";

const token = () => localStorage.getItem("token");

export const getNews = async () => {
  const response = await axios.get(API, {
    headers: {
      Authorization: `Bearer ${token()}`
    }
  });

  return response.data;
};

export const getNewsById = async (id) => {
  const response = await axios.get(`${API}/${id}`, {
    headers: {
      Authorization: `Bearer ${token()}`
    }
  });

  return response.data;
};

export const createNews = async (formData) => {
  const response = await axios.post(API, formData, {
    headers: {
      Authorization: `Bearer ${token()}`
    }
  });

  return response.data;
};

export const updateNews = async (id, formData) => {
  const response = await axios.put(`${API}/${id}`, formData, {
    headers: {
      Authorization: `Bearer ${token()}`
    }
  });

  return response.data;
};