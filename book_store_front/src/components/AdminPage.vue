<template>
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
            users: []
        }
    },

    methods: {
        async deleteUser(id){
            console.log(id);
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