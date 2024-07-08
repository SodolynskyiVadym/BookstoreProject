<template>
    <div class="main-section">
        <label key="email">Email</label><br>
        <input type="text" id="email" v-model="email" @input="checkActive"><br>
        <label key="role">Role</label><br>
        <select id="role" v-model="role" @input="checkActive">
            <option value="EDITOR">EDITOR</option>
            <option value="ADMIN">ADMIN</option>
        </select><br>
        <button @click="registerUser" :class="{ 'main-button': isActive, 'main-button-disabled': !isActive }" :disabled="!isActive">Register</button>
    </div>
    <table class="users-section">
        <thead>
            <tr style="font-size: x-large; background-color: antiquewhite;">
                <th>Name</th>
                <th>Email</th>
                <th>Role</th>
                <th style="padding-left: 200px; padding-right: 200px">Action</th>
            </tr>
        </thead>
            <tr v-for="user in users" :key="user.id" class="user">
                <td style="width: 200px;">{{ user.name }}</td>
                <td style="width: 350px;">{{ user.email }}</td>
                <td style="width: 130px;">{{ user.role }}</td>
                <td style="padding-left: 200px; padding-right: 200px"><button @click="deleteUser(user.id)">DELETE</button></td>
            </tr>
    </table>
</template> 

<script>
import * as listURL from "@/js/listUrl";


export default {
    data(){
        return{
            email: "",
            role: "EDITOR",
            users: [],
            isActive: false
        }
    },

    methods: {
        async checkEmail(email) {
            const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
            return emailRegex.test(email);
        },


        async checkActive(){
            if((this.role === "ADMIN" || this.role === "EDITOR") && await this.checkEmail(this.email)) this.isActive = true;
            else this.isActive = false;
        },


        async deleteUser(id){
            const token = localStorage.getItem("token");
            await listURL.deleteUser(id, token);
            this.users = this.users = await listURL.getAllUsers();
        },


        async registerUser(){
            this.isActive = false;
            const data = {
                email : this.email,
                role : this.role
            }
            const token = localStorage.getItem("token");

            await listURL.postRegistrationWorker(data, token);

            this.users = this.users = await listURL.getAllUsers();
            this.email = "";
            this.role = "EDITOR";

        }
    },

    async mounted(){
        this.users = await listURL.getAllUsers();
    }
}
</script>

<style>
@import '@/assets/css/styles.css';

</style>