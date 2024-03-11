import { createRouter, createWebHistory } from 'vue-router';
// import * as listURL from "@/js/listURL";

const routes = [
    {
        path: '/',
        name: 'HomePage',
        component: () => import('./src/components/HomePage.vue')
    },
    {
        path: '/bookView',
        name: 'BookView',
        component: () => import('./src/components/BookView.vue')
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