<template>

    <div class="create-book-section">
        <label key="name">Name</label>
        <input id="name" type="text" v-model="name" placeholder="Name" @change="checkIsActiveButton">
        <label key="language">Language</label>
        <input id="language" type="text" v-model="bookLanguage" placeholder="Language" @change="checkIsActiveButton">
        <label key="price">Price</label>
        <input id="price" type="number" v-model="price" min="0" @change="checkIsActiveButton">
        <label key="discount">Discount</label>
        <input id="discount" type="number" v-model="discount" min="0" max="100" @change="checkIsActiveButton">
        <label key="authorName">Author</label>
        <select id="authorName" v-model="authorName" @change="findIdByNameAuthor">
            <option v-if="authors.length === 0" disabled>No authors available</option>
            <option v-for="author in authors" :key="author.id" v-text="author.name"></option>
        </select>
        <label key="publisherName">Publisher</label>
        <select id="publisherName" v-model="publisherName" @change="findIdByNamePublisher">
            <option v-if="publishers.length === 0" disabled>No publishers available</option>
            <option v-for="publisher in publishers" :key="publisher.id" v-text="publisher.name"></option>
        </select>
        <label key="numberPages">Pages number</label>
        <input id="numberPages" type="number" v-model="numberPages" @change="checkIsActiveButton">
        <label key="yearPublication">Year publication</label>
        <input id="yearPublication" type="Date" v-model="yearPublication" :max="maxDate" @change="checkIsActiveButton">
        <label key="description">Description</label>
        <textarea v-model="description" id="description" placeholder="Description" @change="checkIsActiveButton"></textarea>
        <button @click="createBook" :class="{ 'button-create': isActive, 'button-create-disabled': !isActive }" :disabled="!isActive">Create</button>
    </div>
</template>

<script>
import * as listURL from "@/js/listUrl";

export default {
    data() {
        return {
            name: "",
            description: "",
            numberPages: 0,
            bookLanguage: "",
            yearPublication: this.formatDate(Date.now()),
            maxDate: this.formatDate(Date.now()),
            publisherId: 0,
            publisherName: "",
            authorId: 0,
            authorName: "",
            availableQuantity: 0,
            price: 0,
            discount: 0,
            publishers: [],
            authors: [],
            isActive: false
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


        async findIdByNameAuthor(){
            this.authorId = this.authors.find(author => author.name === this.authorName).id;
            console.log(this.authorId);
        },


        async findIdByNamePublisher(){
            this.publisherId = this.publishers.find(publsher => publsher.name === this.publisherName).id;
            console.log(this.publisherId);
        },


        async checkIsActiveButton(){
            if(this.name && this.description && this.numberPages >= 0 && this.bookLanguage && this.publisherId > 0 && this.authorName && this.authorId > 0
            && this.publisherName && this.price >= 0 && this.discount >= 0 && this.discount <= 100) {
                this.isActive = true;
            }else{
                this.isActive = false;
            }
        },


        async createBook(){
            
            const data = {
                name:  this.name,
                description:  this.description,
                numberPages: this.numberPages,
                bookLanguage:  this.bookLanguage,
                yearPublication: this.yearPublication,
                publisherId: this.publisherId,
                authorId: this.authorId,
                availableQuantity: this.availableQuantity,
                price: this.price,
                discount: this.discount,
            };

            const token = localStorage.getItem("token");
            await listURL.requestPostCreateBook(data, token);

            this.isActive = false;
            this.name = "";
            this.description = "";
            this.numberPages = 0;
            this.bookLanguage = "";
            this.yearPublication = this.formatDate(Date.now());
            this.publisherName = "";
            this.authorName = "";
            this.availableQuantity = 0;
            this.price = 0;
            this.discount = 0;
        }
    },

    async mounted(){
        this.publishers = await listURL.requestGetAllPublishers();
        this.authors = await listURL.requestGetAllAuthors();
    }
}
</script>

<style>

.create-book-section {
    padding-top: 120px;
    justify-content: center;
    display: grid;

}

.create-book-section label {
    font-size: x-large;
}

.create-book-section select {
    font-size: x-large;
    width: 650px;
    height: 50px;
    display: block;
    margin-bottom: 20px;
    border-radius: 15px;
}

.create-book-section input {
    font-size: x-large;
    width: 650px;
    height: 50px;
    display: block;
    margin-bottom: 20px;
    border-radius: 15px;
}

.create-book-section textarea {
    font-size: x-large;
    width: 650px;
    height: 150px;
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

.button-create-disabled {
    color: white;
    position: center;
    width: 100px;
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