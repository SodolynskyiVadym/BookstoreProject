import { createRouter, createWebHistory } from 'vue-router';
import * as listURL from "@/js/listUrl";

const routes = [
    {
        path: '/',
        name: 'MainPage',
        component: () => import('./src/components/MainPage.vue')
    },
    {
        path: '/bookView/:id',
        name: 'BookView',
        component: () => import('./src/components/BookView.vue')
    },
    {
        path: '/author/:id',
        name: 'AuthorPage',
        component: () => import('./src/components/AuthorPage.vue')
    },
    {
        path: '/createBook',
        name: "CreateBook",
        component: () => import('./src/components/CreateBookPage.vue'),
        meta: {
            requiresAuth: true,
            roles: ["ADMIN", "EDITOR"]
        }
    },
    {
        path: '/createAuthor',
        name: "CreateAuthor",
        component: () => import('./src/components/CreateAuthorPage.vue'),
        meta: {
            requiresAuth: true,
            roles: ["ADMIN", "EDITOR"]
        }
    },
    {
        path: '/updateBook/:id',
        name: "UpdateBook",
        component: () => import('./src/components/UpdateBookPage.vue'),
        meta: {
            requiresAuth: true,
            roles: ["ADMIN", "EDITOR"]
        }
    },
    {
        path: '/updateAuthor/:id',
        name: "UpdateAuthor",
        component: () => import('./src/components/UpdateAuthorPage.vue'),
        meta: {
            requiresAuth: true,
            roles: ["ADMIN", "EDITOR"]
        }
    },
    {
        path: '/publisher/:id',
        name: "PublisherView",
        component: () => import('./src/components/PublisherPage.vue')
    },
    {
        path: '/updatePublisher/:id',
        name: "UpdatePublisher",
        component: () => import('./src/components/UpdatePublisherPage.vue'),
        meta: {
            requiresAuth: true,
            roles: ["ADMIN", "EDITOR"]
        }
    },
    {
        path: '/createPublisher',
        name: "CreatePublisher",
        component: () => import('./src/components/CreatePublisherPage.vue'),
        meta: {
            requiresAuth: true,
            roles: ["ADMIN", "EDITOR"]
        }
    },
    {
        path: '/login',
        name: "Login",
        component: () => import('./src/components/LoginPage.vue')
    },
    {
        path: '/registration',
        name: "Registration",
        component: () => import('./src/components/RegistrationPage.vue')
    },
    {
        path: '/adminPage',
        name: "AdminPage",
        component: () => import('./src/components/AdminPage.vue'),
        meta: {
            requiresAuth: true,
            roles: ["ADMIN"]
        }
    },
    {
        path: '/myPage',
        name: "MyPage",
        component: () => import('./src/components/UserPage.vue'),
        meta: {
            requiresAuth: true,
            roles: ["ADMIN", "EDITOR", "USER"]
        }
    },
    {
        path: '/orderPage',
        name: "OrderPage",
        component: () => import('./src/components/OrderPage.vue')
    }
];



const router = createRouter({
    history: createWebHistory(),
    routes
});



router.beforeEach(async (to, from, next) => {
    if (to.meta.requiresAuth) {
        const token = localStorage.getItem('token');
        if (token) {
            const userData = await listURL.requestGetUserByToken(token);
            const role = userData.role;

            if (to.meta.roles.includes(role)) {
                next();
            } else {
                next("/login");
            }
        } else {
            next('/login');
        }
    }else {
        next();
    }
});

export default router;