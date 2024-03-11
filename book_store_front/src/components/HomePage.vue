<template>
    <div v-for="book in books" :key="book.id">
        <div>{{ book.id }}</div>
        <div>{{ book.name }}</div>
        <div>{{ book.price }}</div>
        <div>{{ book.discount }}</div>
        <div>{{ book.availableQuantity }}</div>
        <div>{{ book.likes }}</div>
    </div>
</template>
<script>
import * as listURL from "@/js/listUrl";


export default {
    data() {
    return {
        books: [],
        userID: "",
        userGroupsID: []
    }
    },
    
    methods: {
    async enterGroupPage(groupID) {
        this.$router.push(`/overviewGroup/${groupID}`)
    }
    },

    async mounted(){
        this.books = await listURL.requestGetBooks();
  }
}
</script>
