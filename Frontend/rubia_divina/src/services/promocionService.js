import api from './api'

export const getPromociones = () =>
    api.get('/promociones')

export const createPromocion = (payload) =>
    api.post('/promociones', payload)

export const updatePromocion = (id,payload) =>
    api.put(`/promociones/${id}`,payload)

export const deletePromocion = (id) =>
    api.delete(`/promociones/${id}`)