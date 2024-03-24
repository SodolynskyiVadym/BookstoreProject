<template>
    <div class="register-section">
        <label key="email">Email</label><br>
        <input type="text" id="email" v-model="email" @input="checkActive"><br>
        <label key="role">Role</label><br>
        <select id="role" v-model="role" @input="checkActive">
            <option value="EDITOR">EDITOR</option>
            <option value="ADMIN">ADMIN</option>
        </select><br>
        <button @click="registerUser" :class="{ 'register-button': isActive, 'register-button-disabled': !isActive }" :disabled="!isActive">Register</button>
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
            await listURL.requestDeleteUser(id, token);
            this.users = this.users = await listURL.requestGetAllUsers();
        },


        async registerUser(){
            this.isActive = false;
            const data = {
                email : this.email,
                role : this.role
            }
            const token = localStorage.getItem("token");

            await listURL.requestPostRegistrationWorker(data, token);

            this.users = this.users = await listURL.requestGetAllUsers();
            this.email = "";
            this.role = "EDITOR";

        }
    },

    async mounted(){
        this.users = await listURL.requestGetAllUsers();
    }
}
</script>

<style>

.users-section {
    padding-top: 140px;
    margin-left: 22%;
    text-align: center;
}


.user {
    font-size: large;
    height: 70px;
    background-color: rgb(202, 202, 202);
}

.register-section {
    padding-top: 100px;
    text-align: center;
}

.register-section label {
    font-size: x-large;
}

.register-section input, select {
    font-size: x-large;
    width: 650px;
    height: 50px;
    margin-bottom: 20px;
    border-radius: 15px;
}


.register-button{
    color: white;
    width: 160px;
    height: 50px;
    text-align: center;
    background-color: rgb(0, 0, 0);
    border-radius: 5px;
    cursor: pointer;
    font-weight: blod;
    font-size: x-large;
    border: none;
}


.register-button-disabled {
    color: white;
    width: 160px;
    height: 50px;
    text-align: center;
    background-color: rgb(158, 158, 158);
    border-radius: 5px;
    cursor: pointer;
    font-weight: blod;
    font-size: x-large;
    border: none;
}


.user button {
    color: white;
    position: center;
    height: 30px;
    text-align: center;
    background-color: rgb(0, 0, 0);
    border-radius: 5px;
    cursor: pointer;
    font-weight: blod;
    font-size: 1em;
    border: none;
}

</style>