import axios from 'axios';

const API_URL = 'http://localhost:5000/api';

// Create an axios instance
const api = axios.create({
  baseURL: API_URL,
});

// Add request interceptor to attach token to requests if available
api.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('token');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

// Authentication
export const registerUser = (userData: { username: string; email: string; password: string }) => {
  return api.post('/auth/register', userData);
};

export const loginUser = (credentials: { email: string; password: string }) => {
  return api.post('/auth/login', credentials);
};

// Contacts
export const getAllContacts = () => {
  return api.get('/contact');
};

export const getContactById = (id: number) => {
  return api.get(`/contact/${id}`);
};

export const createContact = (contactData: any) => {
  return api.post('/contact', contactData);
};

export const updateContact = (id: number, contactData: any) => {
  return api.patch(`/contact/${id}`, contactData);
};

export const deleteContact = (id: number) => {
  return api.delete(`/contact/${id}`);
};

export default api; 