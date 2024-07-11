import axios from "axios";
import router from "../../../router";

const mainUrl = "http://localhost:5224/author";



export async function getAuthor(id) {
    try {
        return await axios.get(mainUrl + "/getAuthor/" + id).then(res => res.data);
    } catch (error) {
        console.error("Error fetching author:", error);
        await router.push("/error");
    }
}


export async function getAuthorBooks(id) {
    try {
        return await axios.get(mainUrl + "/getAuthorBooks/" + id).then(res => res.data);
    } catch (error) {
        console.error("Error fetching author's books:", error);
        await router.push("/error");
    }
}


export async function getAllAuthors() {
    try {
        return await axios.get(mainUrl + "/getAllAuthors").then(res => res.data);
    } catch (error) {
        console.error("Error fetching all authors:", error);
        await router.push("/error");
    }
}


// Додати в authorAPI.js
export async function postCreateAuthor(data, token) {
    const config = {
        headers: {
            'Authorization': `Bearer ${token}`
        }
    };

    try {
        return await axios.post(mainUrl + "/createAuthor", data, config).then(res => res.data);
    } catch (error) {
        console.error("Error creating author:", error);
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
        return await axios.post(mainUrl + "/createImageAuthor", data, config).then(res => res.data);
    } catch (error) {
        console.error("Error creating author image:", error);
        await router.push("/error");
    }
}

export async function patchUpdateAuthor(data) {
    try {
        return await axios.patch(mainUrl + "/updateAuthor", data).then(res => res.data);
    } catch (error) {
        console.error("Error updating author:", error);
        await router.push("/error");
    }
}