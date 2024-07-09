import axios from "axios";
import router from "../../../router";

const mainUrl = "http://localhost:5224/auth";



export async function getUserByToken(token) {
    const config = {
        headers: {
            'Authorization': `Bearer ${token}`
        }
    };

    try {
        return await axios.get(mainUrl + "/getUserByToken", config).then(res => res.data);
    } catch (error) {
        console.error("Error fetching user by token:", error);
        await router.push("/error");
    }
}


export async function getAllUsers() {
    try {
        return await axios.get(mainUrl + "/getAllUsers");
    } catch (error) {
        console.error("Error fetching all users:", error);
        await router.push("/error");
    }
}


// Додати в authAPI.js
export async function patchUpdateUser(data, token) {
    const config = {
        headers: {
            'Authorization': `Bearer ${token}`
        }
    };

    try {
        return await axios.patch(mainUrl + "/updateUser", data, config).then(res => res.data);
    } catch (error) {
        console.error("Error updating user:", error);
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
        return await axios.post(mainUrl + "/registerWorker", data, config).then(res => res.data);
    } catch (error) {
        console.error("Error registering worker:", error);
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
        return await axios.delete(mainUrl + "/deleteUser/" + id, config).then(res => res.data);
    } catch (error) {
        console.error("Error deleting user:", error);
        await router.push("/error");
    }
}