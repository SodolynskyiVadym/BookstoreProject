import axios from "axios";
import router from "../../../router";

const mainUrl = "http://localhost:5224/publisher";


export async function getPublisher(id) {
    try {
        return await axios.get(mainUrl + "/getPublisher/" + id).then(res => res.data);
    } catch (error) {
        console.error("Error fetching publisher:", error);
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


export async function getAllPublishers() {
    try {
        return await axios.get(mainUrl + "/getAllPublishers");
    } catch (error) {
        console.error("Error fetching all publishers:", error);
        await router.push("/error");
    }
}


// Додати в publisherAPI.js
export async function postCreatePublisher(data, token) {
    const config = {
        headers: {
            'Authorization': `Bearer ${token}`
        }
    };

    try {
        return await axios.post(mainUrl + "/createPublisher", data, config).then(res => res.data);
    } catch (error) {
        console.error("Error creating publisher:", error);
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
        return await axios.post(mainUrl + "/createImagePublisher", data, config).then(res => res.data);
    } catch (error) {
        console.error("Error creating publisher image:", error);
        await router.push("/error");
    }
}

export async function patchUpdatePublisher(data) {
    try {
        return await axios.patch(mainUrl + "/updatePublisher", data).then(res => res.data);
    } catch (error) {
        console.error("Error updating publisher:", error);
        await router.push("/error");
    }
}