<template>
  <div v-if="loaded">
    <div class="author-nameBook">
      <div>{{ book.authorName }}</div>
      <div>Book "{{ book.name }}"</div>
    </div>
    <div class="book-info">
      <img class="img-book"
           :src="require(`@/assets/bookPhoto/${book.name.toLowerCase().replace(/\s+/g, '')}${book.id}.jpg`)">
      <div class="book-info-section" style="border-top-left-radius: 15px; border-bottom-left-radius: 15px;">
        <br><br><br>
        <div>Name ----------------</div>
        <div>Author --------------</div>
        <div>Year publication ----</div>
        <div>Pages ---------------</div>
        <div>Language ------------</div>
        <div>Publisher -----------</div>
        <div>Price ---------------</div>
        <div>Discount ------------</div>
      </div>
      <div class="book-info-section"
           style="margin-left: 0px; border-bottom-right-radius: 15px; border-top-right-radius: 15px;">
        <div class="isBookStyle" v-if="isInStock">THE BOOK IS IN STOCK</div>
        <div class="isNotBookStyle" v-else>THE BOOK IS NOT IN STOCK</div>
        <div>{{ book.name }}</div>
        <a @click="enterAuthorPage" style="color: blue; cursor: pointer; text-decoration: underline;">
          <div>{{ book.authorName }}</div>
        </a>
        <div>{{ book.yearPublication }}</div>
        <div>{{ book.numberPages }}</div>
        <div>{{ book.bookLanguage }}</div>
        <div @click="enterPublisherPage" style="color: blue; cursor: pointer; text-decoration: underline;">
          {{ book.publisherName }}
        </div>
        <div>{{ book.price }}</div>
        <div>{{ book.discount }}</div>
      </div>
      <div class="buy-section">
        <div style="margin: 15px;">{{ book.authorName + " - <" + book.name + ">" }}</div>
        <br><br><br>
        <div style="display: flex; justify-content: space-between;">
          <div style="margin-left: 30px;">Price</div>
          <div style="text-align: right; margin-right: 30px;">
            {{ (book.price - (book.price * book.discount / 100)).toFixed(0) }} UAH
          </div>
        </div>

        <div style="display: flex; justify-content: space-between; margin-top: 20px;">
          <input type="number" v-if="isInStock" style="margin-left: 30px; font-size: large;" min="1"
                 :max="book.availableQuantity" v-model="orderedQuantity" :value="orderedQuantity"
                 @change="changeQuantityOrder()">
          <button class="button-buy" @click="enterBookUpdatePage" v-if="isEditorAdmin">Update</button>
          <div v-if="isInStock">
            <button v-if="isNoOrdered" @click="changeOrCreateOrder(book.id)" class="button-buy">Buy</button>
            <button v-else class="button-buy" @click="cancelOrder(book.id)">Cancel</button>
          </div>

        </div>
      </div>
    </div>
    <div class="description">
      <text>
        {{ book.description }}
      </text>
    </div>
  </div>

  <div class="review-section" v-if="reviews.length > 0">
    <div v-for="review in reviews" :key="review.id">
      <div>
        <div>{{ review.userName }}</div>
        <div>{{ review.mark }} <img style="width: 40px; height: 40px" src="../assets/star.png"></div>
        <div style="text-align: center">{{ review.description }}</div>
      </div>
    </div>
  </div>


  <div v-if="isUser" class="review-create-section">
    <label key="mark">Mark </label>
    <input id="mark" type="number" v-model="userReview.mark" min="0" max="5" @keydown.prevent><br>
    <label>Review</label><br>
    <textarea id="descriptionReview" v-model="userReview.description"></textarea><br>
    <button v-if="isUserReview" @click="updateReview" class="main-button" style="margin-bottom: 40px">Update</button>
    <button v-else @click="createReview" class="main-button" style="margin-bottom: 40px">Create</button>
  </div>
</template>


<script>
import * as reviewAPI from "@/js/API/reviewAPI";
import * as BookAPI from "@/js/API/bookAPI";
import * as authAPI from "@/js/API/authAPI";
import * as orderMaker from "@/js/orderMaker";
import * as dateHelper from "@/js/dateHelper";

export default {
  data() {
    return {
      book: null,
      loaded: false,
      firstOrderedQuantity: 0,
      orderedQuantity: 0,
      isNoOrdered: false,
      isInStock: false,
      reviews: [],
      isUser: false,
      isEditorAdmin: false,
      userReview: {
        description: "",
        mark: 0
      },
      isUserReview: false
    };
  },

  methods: {
    async enterAuthorPage() {
      this.$router.push(`/author/${this.book.authorId}`);
    },


    async cancelOrder(id) {
      await orderMaker.removeBookFromOrder(id);
      this.firstOrderedQuantity = -1;
      this.isNoOrdered = true;
    },

    async changeOrCreateOrder(id) {
      const arrayOrderedBooks = await orderMaker.getOrderedBooksArray();
      if (arrayOrderedBooks.includes(id)) {
        await orderMaker.changeQuantity(id, this.orderedQuantity);
      } else await orderMaker.addBookToOrder(id, this.orderedQuantity);

      this.firstOrderedQuantity = this.orderedQuantity;
      this.isNoOrdered = false;
    },

    async createReview() {
      const data = {
        bookId: this.$route.params.id,
        mark: this.userReview.mark,
        description: this.userReview.description
      }
      const token = localStorage.getItem("token");
      await reviewAPI.postCreateReview(data, token);
      this.reviews = await reviewAPI.getReviews(this.$route.params.id);
      this.isUserReview = true;
    },


    async updateReview() {
      const data = {
        id: this.userReview.id,
        bookId: this.$route.params.id,
        mark: this.userReview.mark,
        description: this.userReview.description
      }
      const token = localStorage.getItem("token");
      await reviewAPI.patchUpdateReview(data, token);
      this.reviews = await reviewAPI.getReviews(this.$route.params.id);
    },

    async changeQuantityOrder() {
      this.isNoOrdered = this.firstOrderedQuantity !== this.orderedQuantity;
    },


    async enterBookUpdatePage() {
      this.$router.push(`/updateBook/${this.$route.params.id}`);
    },


    async enterPublisherPage() {
      this.$router.push(`/publisher/${this.book.publisherId}`);
    }
  },

  async mounted() {
    this.book = await BookAPI.getBook(this.$route.params.id);
    this.reviews = await reviewAPI.getReviews(this.$route.params.id);
    for(let review of this.reviews){
      console.log(review)
    }
    const token = localStorage.getItem("token");
    if (token) {
      const review = await reviewAPI.getReviewUserBook(this.$route.params.id, token);
      if (review){
        this.userReview = review;
        this.isUserReview = true;
      }

      const user = await authAPI.getUserByToken(token);
      if (user.role) {
        this.isUser = true;
      }
      if (user.role === "EDITOR" || user.role === "ADMIN") this.isEditorAdmin = true;
    }

    const orderBookQuantity = await orderMaker.getOrderBookIdQuantity();
    const orderedBooks = await orderMaker.getOrderedBooksArray();

    this.orderedQuantity = orderBookQuantity[this.book.id] ?? 1;
    this.firstOrderedQuantity = orderBookQuantity[this.book.id] ?? -1;

    this.isNoOrdered = !orderedBooks.includes(this.book.id);
    this.isInStock = this.book.availableQuantity > 0;
    this.book.yearPublication = dateHelper.formatDate(this.book.yearPublication);
    this.loaded = true;
    console.log(this.reviews);
  }
}
</script>

<style>
@import "@/assets/css/styles.css";
@import "@/assets/css/bookView.css";

</style>