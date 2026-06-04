import api from './api'

export const obtenerAnalitica = async () => {
  const response = await api.get('/analitica')
  return response.data
}