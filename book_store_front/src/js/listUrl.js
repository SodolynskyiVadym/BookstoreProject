import axios from "axios";
import router from "../../router";

const mainUrl = "http://localhost:5224";





























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


