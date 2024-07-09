import axios from "axios";
import router from "../../../router";

const mainUrl = "http://localhost:5224/review";


export async function getReviews(id) {
    try {
        return await axios.get(mainUrl + "/getReviews/" + id).then(res => res.data);
    } catch (error) {
        console.error("Error fetching reviews:", error);
        await router.push("/error");
    }
}


export async function getReviewUserBook(bookId, token) {
    const config = {
        headers: {
            'Authorization': `Bearer ${token}`
        }
    };

    try {
        return await axios.get(mainUrl + "/GetUserReview/" + bookId, config).then(res => res.data);
    } catch (error) {
        console.error("Error fetching review for user and book:", error);
        await router.push("/error");
    }
}



export async function postCreateReview(data, token) {
    const config = {
        headers: {
            'Authorization': `Bearer ${token}`
        }
    };

    try {
        return await axios.post(mainUrl + "/createReview", data, config).then(res => res.data);
    } catch (error) {
        console.error("Error creating review:", error);
        await router.push("/error");
    }
}


export async function patchUpdateReview(data, token) {
    const config = {
        headers: {
            'Authorization': `Bearer ${token}`
        }
    };

    try {
        return await axios.patch(mainUrl + "/updateReview", data, config).then(res => res.data);
    } catch (error) {
        console.error("Error updating review:", error);
        await router.push("/error");
    }
}