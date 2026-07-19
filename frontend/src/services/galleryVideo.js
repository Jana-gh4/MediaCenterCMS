import axios from "axios";

const API = "http://localhost:5202/api/GalleryVideo";

const token = () => localStorage.getItem("token");

const auth = () => ({
  headers: {
    Authorization: `Bearer ${token()}`
  }
});

export const getVideos = async () => {
  const { data } = await axios.get(API, auth());
  return data;
};

export const getVideoById = async (id) => {
  const { data } = await axios.get(`${API}/${id}`, auth());
  return data;
};

export const createVideo = async (formData) => {
  const { data } = await axios.post(API, formData, auth());
  return data;
};

export const hideVideo = async (id) => {
  await axios.put(`${API}/${id}/hide`, {}, auth());
};

export const showVideo = async (id) => {
  await axios.put(`${API}/${id}/show`, {}, auth());
};