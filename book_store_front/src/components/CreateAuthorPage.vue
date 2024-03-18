<template>
    <div class="create-author-section">
        <label key="name">Name</label>
        <input type="text" v-model="name" id="name" placeholder="Name">
        <label key="biography">Biography</label>
        <textarea v-model="biography" id="biography"></textarea>

        <input type="checkbox" v-model="isKnownBirth" id="isKnownBirth">
        <label for="isKnownBirth">Birth date</label>
        <input type="date" v-model="birthYear" id="birthYear" :max="deathYear" :disabled="!isKnownBirth">

        <input class="checkbox-style" type="checkbox" v-model="isKnownDeath" id="isKnownDeath">
        <label for="isKnownDeath">Death date</label>
        <input type="date" v-model="deathYear" id="deathYear" :disabled="!isKnownDeath">

        <button @click="createAuthor" class="button-create">Create</button>
    </div>
</template>

<script>
// import * as listURL from "@/js/listUrl";


export default {
    data(){
        return{
            name: "",
            biography: "",
            birthYear: this.formatDate(Date.now()),
            deathYear: this.formatDate(Date.now()),
            isKnownBirth: false,
            isKnownDeath: false
        }
    },

    methods: {
        formatDate(date) {
            const d = new Date(date);
            const day = d.getDate().toString().padStart(2, '0');
            const month = (d.getMonth() + 1).toString().padStart(2, '0');
            const year = d.getFullYear();
            return `${year}-${month}-${day}`;
        },


        async createAuthor(){
            const dateBirth = this.isKnownBirth ? this.birthYear : null;
            const dateDeath = this.isKnownDeath ? this.deathYear : null;

            const data = {
                name: this.name,
                biography: this.biography,
                birthYear: dateBirth,
                deathYear: dateDeath
            }
            await listURL.requestPostCreateAuthor(data);
        }
    },

    async mounted(){

    }
}
</script>

<style>

.create-author-section {
    padding-top: 120px;
    justify-content: center;
    display: grid;
}


.create-author-section label {
    font-size: x-large;
}

.create-author-section input[type="text"],
.create-author-section input[type="date"] {
    font-size: x-large;
    width: 650px;
    height: 50px;
    display: block;
    margin-bottom: 20px;
    border-radius: 15px;
}


.create-author-section input[type="checkbox"]{
    transform: scale(2);
}

.create-author-section textarea {
    font-size: x-large;
    width: 650px;
    height: 250px;
    display: block;
    margin-bottom: 20px;
    border-radius: 15px;
}

.button-create {
    color: white;
    position: center;
    width: 100px;
    height: 50px;
    text-align: center;
    background-color: rgb(0, 0, 0);
    border-radius: 15px;
    cursor: pointer;
    font-weight: blod;
    font-size: 1.2em;
    border: none;
}

.button-create:hover {
    background-color: rgb(48, 45, 45);
}

</style>