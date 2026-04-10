import api from './api'

export const getProductos = () => api.get('/productos')
export const createProducto = (payload) => api.post('/productos', payload)
export const updateProducto = (id, payload) => api.put(`/productos/${id}`, payload)
export const deleteProducto = (id) => api.delete(`/productos/${id}`)
