import api from './api'

export const getDashboard = () =>
    api.get('/dashboard')

export const getProductosMasVendidosDia = async () => {
    return await api.get('/dashboard/producto-mas-vendido-dia');
}