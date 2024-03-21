import { createRouter, createWebHistory } from 'vue-router';
// import * as listURL from "@/js/listURL";

const routes = [
    {
        path: '/',
        name: 'HomePage',
        component: () => import('./src/components/HomePage.vue')
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
        component: () => import('./src/components/CreateBookPage.vue')
    },
    {
        path: '/createAuthor',
        name: "CreateAuthor",
        component: () => import('./src/components/CreateAuthorPage.vue')
    },
    {
        path: '/updateBook/:id',
        name: "UpdateBook",
        component: () => import('./src/components/UpdateBookPage.vue')
    },
    {
        path: '/updateAuthor/:id',
        name: "UpdateAuthor",
        component: () => import('./src/components/UpdateAuthorPage.vue')
    },
    {
        path: '/publisher/:id',
        name: "PublisherView",
        component: () => import('./src/components/PublisherPage.vue')
    }
];



const router = createRouter({
    history: createWebHistory(),
    routes
});



// router.beforeEach(async (to, from, next) => {
//     if (to.meta.requiresAuth) {
//         const token = localStorage.getItem('token');
//         if (token) {
//             const userData = await listURL.getUserByToken(localStorage.getItem("token"));
//             const role = userData.role;

//             if (to.meta.roles.includes(role)) {
//                 next();
//             } else {
//                 next("/login");
//             }
//         } else {
//             next('/login');
//         }
//     }else {
//         next();
//     }
// });

export default router;