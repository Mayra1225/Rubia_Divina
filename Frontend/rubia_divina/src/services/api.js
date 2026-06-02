import axios from 'axios'
import { getToken } from './storageService'

const api = axios.create({
    baseURL: 'https://rubiadivina-login-crud-mvc-vuenet.onrender.com/api',
})

api.interceptors.request.use(
  (config) => {
    const token = getToken()

    if (token) {
      config.headers.Authorization = `Bearer ${token}`
    }

    return config
  },
  (error) => {
    return Promise.reject(error)
  },
)

export default api
