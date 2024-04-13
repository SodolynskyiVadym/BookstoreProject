import axios from "axios";
import router from "../../router";

const mainUrl = "http://localhost:5224";



export async function getUserByToken(token) {
    const config = {
        headers: {
            'Authorization': `Bearer ${token}`
        }
    };

    try {
        return await axios.get(mainUrl + "/auth/getUserByToken", config).then(res => res.data);
    } catch (error) {
        console.error("Error fetching user by token:", error);
        await router.push("/error");
    }
}

export async function getBooks() {
    try {
        return await axios.get(mainUrl + "/book/getAllBooks").then(res => res.data);
    } catch (error) {
        console.error("Error fetching books:", error);
        await router.push("/error");
    }
}

export async function getReviews(id) {
    try {
        return await axios.get(mainUrl + "/review/getReviews/" + id).then(res => res.data);
    } catch (error) {
        console.error("Error fetching reviews:", error);
        await router.push("/error");
    }
}

export async function getPublisher(id) {
    try {
        return await axios.get(mainUrl + "/publisher/getPublisher/" + id).then(res => res.data);
    } catch (error) {
        console.error("Error fetching publisher:", error);
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
        return await axios.get(mainUrl + "/review/GetUserReview/" + bookId, config).then(res => res.data);
    } catch (error) {
        console.error("Error fetching review for user and book:", error);
        await router.push("/error");
    }
}

export async function getBook(id) {
    try {
        return await axios.get(mainUrl + "/book/getBook/" + id).then(res => res.data);
    } catch (error) {
        console.error("Error fetching book:", error);
        await router.push("/error");
    }
}

export async function getSomeBook(arrayBooks) {
    const strBooksId = arrayBooks.join(",");
    try {
        return await axios.get(mainUrl + "/book/getSomeBooks?ids=" + strBooksId);
    } catch (error) {
        console.error("Error fetching some books:", error);
        await router.push("/error");
    }
}

export async function getAuthorBooks(id) {
    try {
        return await axios.get(mainUrl + "/author/getAuthorBooks/" + id).then(res => res.data);
    } catch (error) {
        console.error("Error fetching author's books:", error);
        await router.push("/error");
    }
}

export async function getPublisherBooks(id) {
    try {
        return await axios.get(mainUrl + "/publisher/getPublisherBooks/" + id).then(res => res.data);
    } catch (error) {
        console.error("Error fetching publisher's books:", error);
        await router.push("/error");
    }
}

export async function getAuthor(id) {
    try {
        return await axios.get(mainUrl + "/author/getAuthor/" + id).then(res => res.data);
    } catch (error) {
        console.error("Error fetching author:", error);
        await router.push("/error");
    }
}

export async function getAllAuthors() {
    try {
        return await axios.get(mainUrl + "/author/getAllAuthors");
    } catch (error) {
        console.error("Error fetching all authors:", error);
        await router.push("/error");
    }
}

export async function getAllPublishers() {
    try {
        return await axios.get(mainUrl + "/publisher/getAllPublishers");
    } catch (error) {
        console.error("Error fetching all publishers:", error);
        await router.push("/error");
    }
}

export async function getAllUsers() {
    try {
        return await axios.get(mainUrl + "/auth/getAllUsers");
    } catch (error) {
        console.error("Error fetching all users:", error);
        await router.push("/error");
    }
}

export async function postCreateBook(data, token) {
    const config = {
        headers: {
            'Authorization': `Bearer ${token}`
        }
    };

    try {
        return await axios.post(mainUrl + "/book/createBook", data, config).then(res => res.data);
    } catch (error) {
        console.error("Error creating book:", error);
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
        return await axios.post(mainUrl + "/review/createReview", data, config).then(res => res.data);
    } catch (error) {
        console.error("Error creating review:", error);
        await router.push("/error");
    }
}

export async function postCreateAuthor(data, token) {
    const config = {
        headers: {
            'Authorization': `Bearer ${token}`
        }
    };

    try {
        return await axios.post(mainUrl + "/author/createAuthor", data, config).then(res => res.data);
    } catch (error) {
        console.error("Error creating author:", error);
        await router.push("/error");
    }
}

export async function postCreatePublisher(data, token) {
    const config = {
        headers: {
            'Authorization': `Bearer ${token}`
        }
    };

    try {
        return await axios.post(mainUrl + "/publisher/createPublisher", data, config).then(res => res.data);
    } catch (error) {
        console.error("Error creating publisher:", error);
        await router.push("/error");
    }
}

export async function patchUpdateUser(data, token) {
    const config = {
        headers: {
            'Authorization': `Bearer ${token}`
        }
    };

    try {
        return await axios.patch(mainUrl + "/auth/updateUser", data, config).then(res => res.data);
    } catch (error) {
        console.error("Error updating user:", error);
        await router.push("/error");
    }
}

export async function postRegistration(data) {
    try {
        return await axios.post(mainUrl + "/auth/registerUser", data).then(res => res.data);
    } catch (error) {
        console.error("Error registering user:", error);
        await router.push("/error");
    }
}

export async function postLogin(data) {
    try {
        return await axios.post(mainUrl + "/auth/login", data).then(res => res.data);
    } catch (error) {
        console.error("Error logging in:", error);
        await router.push("/error");
    }
}

export async function postRegistrationWorker(data, token) {
    const config = {
        headers: {
            'Authorization': `Bearer ${token}`
        }
    };

    try {
        return await axios.post(mainUrl + "/auth/registerWorker", data, config).then(res => res.data);
    } catch (error) {
        console.error("Error registering worker:", error);
        await router.push("/error");
    }
}

export async function postCreateBookImage(data, token) {
    const config = {
        headers: {
            'Authorization': `Bearer ${token}`
        }
    };

    try {
        return await axios.post(mainUrl + "/book/createImageBook", data, config).then(res => res.data);
    } catch (error) {
        console.error("Error creating book image:", error);
        await router.push("/error");
    }
}

export async function postCreatePublisherImage(data, token) {
    const config = {
        headers: {
            'Authorization': `Bearer ${token}`
        }
    };

    try {
        return await axios.post(mainUrl + "/publisher/createImagePublisher", data, config).then(res => res.data);
    } catch (error) {
        console.error("Error creating publisher image:", error);
        await router.push("/error");
    }
}

export async function postCreateAuthorImage(data, token) {
    const config = {
        headers: {
            'Authorization': `Bearer ${token}`
        }
    };

    try {
        return await axios.post(mainUrl + "/author/createImageAuthor", data, config).then(res => res.data);
    } catch (error) {
        console.error("Error creating author image:", error);
        await router.push("/error");
    }
}

export async function patchUpdateBook(data) {
    try {
        return await axios.patch(mainUrl + "/book/updateBook", data).then(res => res.data);
    } catch (error) {
        console.error("Error updating book:", error);
        await router.push("/error");
    }
}

export async function patchUpdateAuthor(data) {
    try {
        return await axios.patch(mainUrl + "/author/updateAuthor", data).then(res => res.data);
    } catch (error) {
        console.error("Error updating author:", error);
        await router.push("/error");
    }
}

export async function patchUpdatePublisher(data) {
    try {
        return await axios.patch(mainUrl + "/publisher/updatePublisher", data).then(res => res.data);
    } catch (error) {
        console.error("Error updating publisher:", error);
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
        return await axios.patch(mainUrl + "/review/updateReview", data, config).then(res => res.data);
    } catch (error) {
        console.error("Error updating review:", error);
        await router.push("/error");
    }
}

export async function deleteUser(id, token) {
    const config = {
        headers: {
            'Authorization': `Bearer ${token}`
        }
    };

    try {
        return await axios.delete(mainUrl + "/auth/deleteUser/" + id, config).then(res => res.data);
    } catch (error) {
        console.error("Error deleting user:", error);
        await router.push("/error");
    }
}


