import axios from 'axios';

axios.interceptors.request.use(
  config => {
    const token = null; //Todo: add logic to get token
    if (token) {
      config.headers['Authorization'] = 'Bearer ' + token;
    }
    // config.headers['Content-Type'] = 'application/json';
    return config;
  },
  error => {
    Promise.reject(error);
  }
);

export class Requester {
  static get = async (url: string) => {
    try {
      return await axios.get(url);
    } catch (e) {
      throw e;
    }
  };
  static post = async (url: string, data: any) => {
    try {
      return await axios.post(url, data);
    } catch (e) {
      throw e;
    }
  };
  static postFile = async (url: string, data: any) => {
    try {
      return await axios.post(url, data, { 'Content-Type': 'multipart/form-data' });
    } catch (e) {
      throw e;
    }
  };
  static put = async (url: string, data: any) => {
    try {
      return await axios.put(url, data);
    } catch (e) {
      throw e;
    }
  };
  static delete = async (url: string) => {
    try {
      return await axios.delete(url);
    } catch (e) {
      throw e;
    }
  };
}
