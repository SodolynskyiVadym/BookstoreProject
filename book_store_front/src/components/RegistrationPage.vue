<template>
    <div class="registration-section">
        <label key="name">Name</label>
        <input type="text" v-model="name" id="name" placeholder="Name" @change="checkIsActive">
        <label key="email">Email</label>
        <input type="text" v-model="email" id="email" placeholder="Email" @change="checkIsActive">
        <label key="password">Password</label>
        <input type="text" v-model="password" id="password" placeholder="Password" @change="checkIsActive">
        <label key="confirmPassword">Confirm password</label>
        <input type="text" v-model="confirmPassword" id="confirmPassword" placeholder="Confirmation password" @change="checkIsActive">

        <button @click="registration" :class="{ 'button-registration': isActive, 'button-registration-disabled': !isActive }" :disabled="!isActive">Registration</button>
    </div>
</template>

<script>
import * as listURL from "@/js/listUrl";


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
            await listURL.requestPostRegistration(data);

            this.$router.push('/');
        }
    }
}
</script>

<style>

.registration-section {
    padding-top: 120px;
    justify-content: center;
    display: grid;
}


.registration-section label {
    font-size: x-large;
}

.registration-section input {
    font-size: x-large;
    width: 650px;
    height: 50px;
    display: block;
    margin-bottom: 20px;
    border-radius: 15px;
}

.button-registration {
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

.button-registration:hover {
    background-color: rgb(48, 45, 45);
}


.button-registration-disabled {
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