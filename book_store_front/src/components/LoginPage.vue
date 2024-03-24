<template>
    <div class="login-section">
        <label key="email">Email</label>
        <input type="text" v-model="email" id="email" placeholder="Email" @change="checkIsActive">
        <label key="password">Password</label>
        <input type="text" v-model="password" id="password" placeholder="Password" @change="checkIsActive">

        <button @click="login" :class="{ 'button-login': isActive, 'button-login-disabled': !isActive }" :disabled="!isActive">Login</button>
    </div>
</template>

<script>
import * as listURL from "@/js/listUrl";


export default {
    data(){
        return{
            email: "",
            password: "",
            isActive: false
        }
    },

    methods: {
        async checkIsActive(){
            if(this.email && this.password){
                this.isActive = true;
            }else{
                this.isActive = false;
            }
        },


        async login(){
            const dataLogin = {
                email: this.email,
                password: this.password
            }
            const data = await listURL.requestPostLogin(dataLogin);
            const token = data.token
            localStorage.setItem("token", token)

            this.$router.push('/');
        }
    }
}
</script>

<style>

.login-section {
    padding-top: 120px;
    justify-content: center;
    display: grid;
}


.login-section label {
    font-size: x-large;
}

.login-section input {
    font-size: x-large;
    width: 650px;
    height: 50px;
    display: block;
    margin-bottom: 20px;
    border-radius: 15px;
}

.button-login {
    color: white;
    position: center;
    width: 180px;
    height: 50px;
    text-align: center;
    background-color: rgb(0, 0, 0);
    border-radius: 15px;
    cursor: pointer;
    font-weight: blod;
    font-size: 1.2em;
    border: none;
}

.button-login:hover {
    background-color: rgb(48, 45, 45);
}


.button-login-disabled {
    color: white;
    position: center;
    width: 180px;
    height: 50px;
    text-align: center;
    background-color: rgb(119, 116, 116);
    border-radius: 15px;
    cursor: pointer;
    font-weight: blod;
    font-size: 1.2em;
    border: none;
}

</style>