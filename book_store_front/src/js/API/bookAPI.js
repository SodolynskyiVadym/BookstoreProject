import axios from "axios";
import router from "../../../router";

const mainUrl = "http://localhost:5224/book";


export async function getBook(id) {
    try {
        return await axios.get(mainUrl + "/getBook/" + id).then(res => res.data);
    } catch (error) {
        console.error("Error fetching book:", error);
        await router.push("/error");
    }
}


export async function getSomeBook(arrayBooks) {
    const strBooksId = arrayBooks.join(",");
    try {
        return await axios.get(mainUrl + "/getSomeBooks?ids=" + strBooksId);
    } catch (error) {
        console.error("Error fetching some books:", error);
        await router.push("/error");
    }
}


export async function getBooks() {
    try {
        return await axios.get(mainUrl + "/getAllBooks").then(res => res.data);
    } catch (error) {
        console.error("Error fetching books:", error);
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
        return await axios.post(mainUrl + "/createBook", data, config).then(res => res.data);
    } catch (error) {
        console.error("Error creating book:", error);
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
        return await axios.post(mainUrl + "/createImageBook", data, config).then(res => res.data);
    } catch (error) {
        console.error("Error creating book image:", error);
        await router.push("/error");
    }
}

export async function patchUpdateBook(data) {
    try {
        return await axios.patch(mainUrl + "/updateBook", data).then(res => res.data);
    } catch (error) {
        console.error("Error updating book:", error);
        await router.push("/error");
    }
}