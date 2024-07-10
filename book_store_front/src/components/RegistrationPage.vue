<template>
    <div class="main-section">
        <label key="name">Name</label><br>
        <input type="text" v-model="name" id="name" placeholder="Name" @change="checkIsActive"><br>
        <label key="email">Email</label><br>
        <input type="text" v-model="email" id="email" placeholder="Email" @change="checkIsActive"><br>
        <label key="password">Password</label><br>
        <input type="text" v-model="password" id="password" placeholder="Password" @change="checkIsActive"><br>
        <label key="confirmPassword">Confirm password</label><br>
        <input type="text" v-model="confirmPassword" id="confirmPassword" placeholder="Confirmation password" @change="checkIsActive"><br>

        <button @click="registration" :class="{ 'main-button': isActive, 'main-button-disabled': !isActive }" :disabled="!isActive">Registration</button>
    </div>
</template>

<script>
import * as authAPI from "@/js/API/authAPI";


export default {
    data(){
        return{
            name: "",
            email: "",
            password: "",
            confirmPassword: "",
            isActive: false
        }
    },

    methods: {
        async checkEmail(email) {
            const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
            return emailRegex.test(email);
        },


        async checkIsActive(){
            if(this.name && await this.checkEmail(this.email) && this.password && this.confirmPassword){
                this.isActive = true;
            }else{
                this.isActive = false;
            }
        },


        async registration(){
            const data = {
                name: this.name,
                email: this.email,
                password: this.password,
                confirmPassword: this.confirmPassword
            }
            console.log(data);
            await authAPI.postRegistrationUser(data);

            this.$router.push('/');
        }
    }
}
</script>

<style>
@import "@/assets/css/styles.css";
</style>