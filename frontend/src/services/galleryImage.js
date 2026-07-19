import axios from "axios";

const API = "http://localhost:5202/api/GalleryImage";

const token = () => localStorage.getItem("token");

const auth = () => ({
  headers: {
    Authorization: `Bearer ${token()}`
  }
});

export const getImages = async () => {
  const { data } = await axios.get(API, auth());
  return data;
};

export const getImageById = async (id) => {
  const { data } = await axios.get(`${API}/${id}`, auth());
  return data;
};

export const createImage = async (formData) => {
  const { data } = await axios.post(API, formData, auth());
  return data;
};

export const hideImage = async (id) => {
  await axios.put(`${API}/${id}/hide`, {}, auth());
};

export const showImage = async (id) => {
  await axios.put(`${API}/${id}/show`, {}, auth());
};