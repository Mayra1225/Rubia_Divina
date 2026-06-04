import { createRouter, createWebHistory } from 'vue-router'

import LoginView from '../views/LoginView.vue'
import RegisterView from '../views/RegisterView.vue'
import DashboardView from '../views/DashboardView.vue'
import PedidosView from '../views/PedidosView.vue'
import PromocionesView from '../views/PromocionesView.vue'

import { clearSession, getToken } from '../services/storageService'

const routes = [
  {
    path: '/',
    redirect: '/login'
  },

  {
    path: '/login',
    name: 'login',
    component: LoginView,
    meta: { guestOnly: true }
  },

  {
    path: '/register',
    name: 'register',
    component: RegisterView,
    meta: { guestOnly: true }
  },

  {
    path: '/dashboard',
    name: 'dashboard',
    component: DashboardView,
    meta: { requiresAuth: true }
  },

  {
    path: '/pedidos',
    name: 'pedidos',
    component: PedidosView,
    meta: { requiresAuth: true }
  },

  {
    path: '/promociones',
    name: 'promociones',
    component: PromocionesView,
    meta: { requiresAuth: true }
  },

  {
    path: '/:pathMatch(.*)*',
    redirect: '/login'
  }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

router.beforeEach((to, from, next) => {

  const token = getToken()

  if (to.meta.requiresAuth && !token) {
    clearSession()
    next('/login')
    return
  }

  if (to.meta.guestOnly && token) {
    next('/dashboard')
    return
  }

  next()
})

export default router