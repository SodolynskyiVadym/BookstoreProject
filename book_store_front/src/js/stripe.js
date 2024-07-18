import { loadStripe } from '@stripe/stripe-js';
import axios from "axios";


let stripePromise;
const public_key = process.env.STRIPE_PUBLIC_KEY;
const initializeStripe = async () => {
    stripePromise = await loadStripe(public_key);
}

initializeStripe();

export const buyBooks = async (order, token) => {
    const config = {
        headers: {
            'Authorization': `Bearer ${token}`
        }
    }
    const id  = await axios.post(`http://localhost:5224/pay/makeOrder`, order, config).then(res => res.data);

    const stripe = await stripePromise;

    await stripe.redirectToCheckout({
        sessionId: id
    });
};