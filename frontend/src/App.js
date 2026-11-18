import { useEffect, useState } from 'react';
import axios from 'axios';
import './App.css';

const API_BASE = "http://localhost:5277/api";

axios.interceptors.request.use((config) => {
    const token = localStorage.getItem("token");
    if (token) {
        config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
});

function App() {
    const [products, setProducts] = useState([]);
    const [categories, setCategories] = useState([]);
    const [name, setName] = useState("");
    const [price, setPrice] = useState("");
    const [stock, setStock] = useState("");
    const [categoryId, setCategoryId] = useState("");
    const [searchText, setSearchText] = useState("");

    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [email, setEmail] = useState("");
    const [isLoggedIn, setIsLoggedIn] = useState(!!localStorage.getItem("token"));
    const [tab, setTab] = useState("login");

    const fetchProducts = async () => {
        const res = await axios.get(`${API_BASE}/Products`);
        setProducts(res.data);
    };
    const fetchCategories = async () => {
        const res = await axios.get(`${API_BASE}/Categories`);
        setCategories(res.data);
    };

    const addProduct = async () => {
        try {
            await axios.post(`${API_BASE}/Products`, {
                name, price: Number(price), stock: Number(stock),
                categoryId: Number(categoryId),
                description: "", imageUrl: "", unit: "pcs"
            });
            fetchProducts();
            setName(""); setPrice(""); setStock(""); setCategoryId("");
        } catch (err) {
            alert("Add product failed");
        }
    };

    const register = async () => {
        try {
            await axios.post(`${API_BASE}/Auth/register`, { username, email, password });
            alert("Register success");
            setTab("login");
        } catch (err) { alert("Register failed"); }
    };
    const login = async () => {
        try {
            const res = await axios.post(`${API_BASE}/Auth/login`, { username, password });
            localStorage.setItem("token", res.data.token);
            setIsLoggedIn(true);
        } catch (err) { alert("Login failed"); }
    };
    const logout = () => {
        localStorage.removeItem("token");
        setIsLoggedIn(false);
    };

    useEffect(() => {
        fetchProducts();
        fetchCategories();
    }, []);

    const catIcons = [
        { label: "Fruits & Vegetables", icon: "🥕" },
        { label: "Bakery", icon: "🥐" },
        { label: "Meat & Seafood", icon: "🐟" },
        { label: "Dairy & Eggs", icon: "🥛" },
        { label: "Deli & Ready Meals", icon: "🍗" },
        { label: "Pantry", icon: "🥫" }
    ]

    return (
        <div className="app">
            {/* 顶部红条标语 */}
            <div className="top-banner">
                CANADIAN owned and operated for 110 years
            </div>

            {/* Header：Logo + Search + Sign In */}
            <header className="header">
                <div className="header-inner">
                    <div className="logo">FreshMarket</div>
                    <div className="search-bar">
                        <select>
                            <option>Groceries</option>
                        </select>
                        <input
                            placeholder="Search"
                            value={searchText}
                            onChange={(e) => setSearchText(e.target.value)}
                        />
                        <button>🔍</button>
                    </div>
                    <div className="auth-area">
                        {isLoggedIn
                            ? <button onClick={logout} className="btn-logout">Sign Out</button>
                            : <div className="guest-text">Hi Guest<br /><button onClick={() => setTab("login")}>Sign In or Register</button></div>
                        }
                    </div>
                </div>
            </header>

            {/* 主导航菜单 */}
            <nav className="main-nav">
                <span>Shop Aisles</span>
                <span>Flyer & Deals</span>
                <span>Discover</span>
                <span>Pharmacy & Wellness</span>
                <div className="nav-right">
                    <span>Past Purchases</span>
                    <span>Favourites</span>
                    <span>Shopping List</span>
                </div>
            </nav>

            {/* Hero大图横幅 */}
            <section className="hero">
                <div className="hero-text-card">
                    <h2>Savour the Season</h2>
                    <p>Kick off the holiday season with fresh finds, baking needs, hosting, celebrating, and limited-time Christmas flavours, all in one place!</p>
                    <button className="hero-btn">Explore Christmas Flavours</button>
                </div>
            </section>

            {/* 图标分类导航（和截图下面一排图标一致） */}
            <section className="icon-categories">
                {catIcons.map((item, idx) => (
                    <div className="icon-cat-item" key={idx}>
                        <div className="icon">{item.icon}</div>
                        <p>{item.label}</p>
                    </div>
                ))}
            </section>

            {/* 登录弹窗/表单 */}
            {!isLoggedIn && (
                <div className="auth-modal">
                    <div className="auth-card">
                        <div className="auth-tabs">
                            <button className={tab === "login" ? "active" : ""} onClick={() => setTab("login")}>Sign In</button>
                            <button className={tab === "register" ? "active" : ""} onClick={() => setTab("register")}>Register</button>
                        </div>
                        <div className="auth-form">
                            <input placeholder="Username" value={username} onChange={(e) => setUsername(e.target.value)} />
                            {tab === "register" && <input placeholder="Email" value={email} onChange={(e) => setEmail(e.target.value)} />}
                            <input type="password" placeholder="Password" value={password} onChange={(e) => setPassword(e.target.value)} />
                            {tab === "login"
                                ? <button onClick={login} className="btn-primary">Sign In</button>
                                : <button onClick={register} className="btn-primary">Register</button>
                            }
                        </div>
                    </div>
                </div>
            )}

            {/* 商品区域（登录后才可以新增） */}
            <main className="container">
                {isLoggedIn && (
                    <div className="add-form">
                        <h3>Add New Product</h3>
                        <div className="form-row">
                            <input placeholder="Product Name" value={name} onChange={(e) => setName(e.target.value)} />
                            <input placeholder="Price" type="number" value={price} onChange={(e) => setPrice(e.target.value)} />
                            <input placeholder="Stock" type="number" value={stock} onChange={(e) => setStock(e.target.value)} />
                            <select value={categoryId} onChange={(e) => setCategoryId(e.target.value)}>
                                <option value="">Select Category</option>
                                {categories.map(c => <option key={c.categoryId} value={c.categoryId}>{c.name}</option>)}
                            </select>
                        </div>
                        <button onClick={addProduct} className="btn-primary">Submit</button>
                    </div>
                )}

                <h3>Products</h3>
                <div className="product-grid">
                    {products.map(p => (
                        <div className="product-card" key={p.productId}>
                            <h4>{p.name}</h4>
                            <p>${p.price}</p>
                            <p>Stock: {p.stock}</p>
                        </div>
                    ))}
                </div>
            </main>
        </div>
    )
}

export default App;
