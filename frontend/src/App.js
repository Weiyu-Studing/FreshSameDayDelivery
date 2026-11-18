import { useEffect, useState } from 'react';
import axios from 'axios';


const API_BASE = "http://localhost:5277/api";

function App() {
    const [products, setProducts] = useState([]);
    const [name, setName] = useState("");
    const [price, setPrice] = useState("");
    const [categoryId, setCategoryId] = useState(5);

    //read
    const fetchProducts = async () => {
        const res = await axios.get(`${API_BASE}/Products`);
        setProducts(res.data);
    }

    //Add 
    const addProduct = async () => {
        await axios.post(`${API_BASE}/Products`, {
            name: name,
            price: Number(price),
            categoryId: categoryId,
            stock: 10,
            description: "MVP TEST PRODUCT",
            imageUrl: "",
            unit: "kg"
        })
        fetchProducts();
    }

    useEffect(() => {
        fetchProducts();
    }, [])

    return (
        <div style={{ padding: "2rem" }}>
            <h1>Fresh Today - UI TEXT</h1>
            <h3>Product List</h3>
            {products.map(p => (
                <div key={p.productId}>
                    {p.name} | ${p.price} | Stock:{p.stock}
                </div>
            ))}

            <h4>Add new product</h4>
            <input placeholder="Product Name" value={name} onChange={e => setName(e.target.value)} />
            <input placeholder="Price" value={price} onChange={e => setPrice(e.target.value)} />
            <button onClick={addProduct}>Submmit</button>
        </div>
    )
}
export default App;

