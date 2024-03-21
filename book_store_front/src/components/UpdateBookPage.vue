<template>

  <div class="update-book-section" v-if="loaded">
      <label key="name">Name</label>
      <input id="name" type="text" v-model="book.name" placeholder="Name" @change="checkIsActiveButton">
      <label key="language">Language</label>
      <input id="language" type="text" v-model="book.bookLanguage" placeholder="Language" @change="checkIsActiveButton">
      <label key="price">Price</label>
      <input id="price" type="number" v-model="book.price" min="0" @change="checkIsActiveButton">
      <label key="discount">Discount</label>
      <input id="discount" type="number" v-model="book.discount" min="0" max="100" @change="checkIsActiveButton">
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
      <input id="numberPages" type="number" v-model="book.numberPages" @change="checkIsActiveButton">
      <label key="yearPublication">Year publication</label>
      <input id="yearPublication" type="Date" v-model="book.yearPublication" :max="maxDate" @change="checkIsActiveButton">
      <label key="description">Description</label>
      <textarea v-model="book.description" id="description" placeholder="Description" @change="checkIsActiveButton"></textarea>
      <button @click="updateBook" :class="{ 'button-update': isActive, 'button-update-disabled': !isActive }" :disabled="!isActive">Update</button>
  </div>
</template>

<script>
import * as listURL from "@/js/listUrl";

export default {
  data() {
      return {
          publisherName: "",
          authorName: "",
          book: null,
          publishers: [],
          authors: [],
          isActive: true,
          loaded: false
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
          this.book.authorId = this.authors.find(author => author.name === this.authorName).id;
          console.log(this.book.authorId);
      },


      async findIdByNamePublisher(){
          this.book.publisherId = this.publishers.find(publsher => publsher.name === this.publisherName).id;
          console.log(this.book.publisherId);
      },


      async checkIsActiveButton(){
          if(this.book.name && this.book.description && this.book.numberPages >= 0 && this.book.bookLanguage && this.book.publisherId > 0 
          && this.book.authorName && this.book.authorId > 0 && this.publisherName && this.book.price >= 0 && this.book.discount >= 0 && this.book.discount <= 100) {
              this.isActive = true;
          }else{
              this.isActive = false;
          }
      },


      async updateBook(){
          
          const data = {
            id: this.book.id,
            name:  this.book.name,
            description:  this.book.description,
            numberPages: this.book.numberPages,
            bookLanguage:  this.book.bookLanguage,
            yearPublication: this.book.yearPublication,
            publisherId: this.book.publisherId,
            authorId: this.book.authorId,
            availableQuantity: this.book.availableQuantity,
            price: this.book.price,
            discount: this.book.discount,
          };

          console.log(data)

          await listURL.requestPatchUpdateBook(data);
      }
  },

  async mounted(){
      this.publishers = await listURL.requestGetAllPublishers();
      this.authors = await listURL.requestGetAllAuthors();
      this.book = await listURL.requestGetBook(this.$route.params.id);

      this.book.yearPublication = this.formatDate(this.book.yearPublication);
      this.authorName = this.authors.find(author => author.id === this.book.authorId).name;
      this.publisherName = this.publishers.find(publisher => publisher.id === this.book.publisherId).name;
      this.loaded = true;
  }
}
</script>

<style>

.update-book-section {
  padding-top: 120px;
  justify-content: center;
  display: grid;

}

.update-book-section label {
  font-size: x-large;
}

.update-book-section select {
  font-size: x-large;
  width: 650px;
  height: 50px;
  display: block;
  margin-bottom: 20px;
  border-radius: 15px;
}

.update-book-section input {
  font-size: x-large;
  width: 650px;
  height: 50px;
  display: block;
  margin-bottom: 20px;
  border-radius: 15px;
}

.update-book-section textarea {
  font-size: x-large;
  width: 650px;
  height: 150px;
  display: block;
  margin-bottom: 20px;
  border-radius: 15px;
}

.button-update {
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

.button-update:hover {
  background-color: rgb(48, 45, 45);
}

.button-update-disabled {
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