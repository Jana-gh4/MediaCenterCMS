import api from "./api";

export const getApprovals = async () => {
    const response = await api.get("/Approval");
    return response.data;
};

export const approveRequest = async (id) => {
    const response = await api.post(`/Approval/${id}/approve`);
    return response.data;
};

export const rejectRequest = async (id, reason) => {
    const response = await api.post(`/Approval/${id}/reject`, {
        reason,
    });
    return response.data;
};