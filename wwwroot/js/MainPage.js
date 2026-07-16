const mainContainer = document.querySelector(".main-container");
const userRole = mainContainer.dataset.role;

const cartList = document.querySelector(".cart-list");
var cartItems = JSON.parse(cartList?.dataset.cartItems);

const productList = document.querySelector(".product-list-container");
var products = JSON.parse(productList?.dataset.productList);

const cartTotalPrice = document.querySelector('.cart-total-price');
const cartProductCount = document.querySelector('.cart-product-count');

const btnConfirmChanges = document.querySelector(".cart-confirm-changes-btn");
const btnClearCart = document.querySelector(".cart-clear-btn");

btnClearCart.addEventListener('click', function () {

    cartItems.forEach(item => {
        changeProductCount(item.productId, item.quantity);
    });

    cartItems.length = 0;

    renderCart();
});

function changeCartItemCountAndTotalPrice() {
    var totalPrice = 0;
    var productCount = 0;

    cartItems.forEach(item => {
        var product = products.find(p => p.id == item.productId);

        totalPrice += item.quantity * product.price;
        productCount += item.quantity;
    });

    cartTotalPrice.textContent = `Итоговая цена: ${totalPrice.toFixed(2).replace('.', ',')} ₽`;
    cartProductCount.textContent = `Количество товаров: ${productCount}`;
}

btnConfirmChanges.addEventListener('click', async function () {

    var cartItemCards = document.querySelectorAll(".cart-item-card")

    cartItemCards.forEach(card => {
        var input = card.querySelector(".quantity-changer");

        if (input == null)
            return;

        var cartItem = cartItems.find(ci => ci.productId == Number(card.dataset.cartProductId));

        cartItem.quantity = Number(input.value);
    });

    var response = await fetch('/Shop/ConfirmChanges', {
        method: 'Post',
        headers: { 'Content-type': 'application/json' },
        body: JSON.stringify(cartItems)
    });

    var data = await response.json();

    if (data.cart && data.products) {
        cartItems = data.cart.cartItems;
        products = data.products;
    }

    cartItems = cartItems.filter(ci => ci.quantity > 0);

    renderProductList();
    renderCart();
});

productList.addEventListener('click', function (event) {
    if (!event.target.classList.contains('btn-add-to-cart'))
        return;

    var btn = event.target;

    var productCard = btn.closest(".product-card");

    if (productCard == null)
        return;

    var productId = Number(productCard.dataset.productId);

    var product = products.find(p => p.id == productId)
    
    if(product.count <= 0)
        return;

    var cartItem = cartItems.find(ci => ci.productId == productId);

    if (cartItem)
        cartItem.quantity++;
    else {
        cartItems.push({
            productId: productId,
            quantity: 1,
            product: { name: productCard.dataset.name }
        });
    }

    changeProductCount(productId, -1);
    renderCart();
});

function renderCart() {
    if (cartList == null)
        return;

    changeCartItemCountAndTotalPrice();

    if (cartItems.length === 0) {
        cartList.innerHTML = '<p>Корзина пуста</p>';
        return;
    }

    if (cartList.innerHTML == "") {
        cartList.innerHTML = '<p>Корзина пуста</p>';
        return;
    }

    cartList.innerHTML = cartItems.map(item => `
        <div class="cart-item-card" data-cart-product-id="${item.productId}">
            <label class="cart-item-info cart-item-title">${products.find(p => p.id == item.productId).name}</label>
            <div class="cart-item-quantity">
                <label>Количество: </label>
                <input class="quantity-changer" type="number" value="${item.quantity}"/>
            </div>
            <button class="cart-remove-product-btn">Убрать из корзины ❌</button>
        </div>
    `).join('');
}

function changeProductCount(productId, changedQuality) {
    if (productList == null || products.length == 0)
        return;

    var product = products.find(p => p.id == productId);

    product.count += Number(changedQuality);

    if(product.count < 0)
        product.count = 0;

    var productCard = productList.querySelector(`.product-card[data-product-id="${productId}"]`);
    if (productCard) {
        var labelProductCount = productCard.querySelector('.product-count');
        labelProductCount.textContent = `Доступно: ${product.count}`;
    }
}

function renderProductList() {
    if (productList == null)
        return;

    if (products.length == 0) {
        productList.innerHTML = "<p>Товаров пока нет!</p>";
        return;
    }

    if (userRole == 'admin')
        productList.innerHTML = products.map(product =>
            `<div class="product-card" data-product-id="${product.id}" data-name="${product.name}">
                <div class="product-card-header">
                    <label class="product-info product-title">${product.name}</label>

                    <div class="product-actions">
                        <a class="btn product-more-info-btn" asp-action="ViewProductInfo"
                            asp-controller="Shop" asp-route-id="${product.id}">ℹ️</a>
                            <form method="get" asp-action="EditProductPage" asp-controller="Shop"
                                asp-route-id="${product.id}">
                                <button type="submit" class="btn product-update-btn">✏️</button>
                            </form>
                            <button type="submit" class="btn product-delete-btn"
                                data-product-id="${product.id}">❌</button>
                    </div>
                </div>

                <label class="product-info product-price">${product.price.toFixed(2).replace('.', ',')} ₽</label>
                <label class="product-info product-count">Доступно: ${product.count}</label>
                <button class="btn btn-add-to-cart">Добавить в корзину 🧺</button>
            </div>`
        ).join('');
    else
        productList.innerHTML = products.map(product =>
            `<div class="product-card" data-product-id="${product.id}" data-name="${product.name}">
                <div class="product-card-header">
                    <label class="product-info product-title">${product.name}</label>

                    <div class="product-actions">
                        <a class="btn product-more-info-btn" asp-action="ViewProductInfo"
                            asp-controller="Shop" asp-route-id="${product.id}">ℹ️</a>
                    </div>
                </div>

                <label class="product-info product-price">${product.price.toFixed(2).replace('.', ',')} ₽</label>
                <label class="product-info product-count">Доступно: ${product.count}</label>
                <button class="btn btn-add-to-cart">Добавить в корзину 🧺</button>
            </div>`
        ).join('');
}

cartList.addEventListener('input', function (event) {
    if (!event.target.classList.contains("quantity-changer"))
        return;

    var input = event.target;

    var productId = Number(input.closest('.cart-item-card').dataset.cartProductId);
    var cartItem = cartItems.find(ci => ci.productId == productId);

    var product = products.find(p => p.id == productId);

    var oldQuantity = cartItem.quantity;
    var maxAllowed = product.count + oldQuantity;
    var newQuantity = Number(input.value);

    if (newQuantity < 0) {
        newQuantity = 0;
        input.value = '0';
    }

    if (newQuantity > maxAllowed) {
        newQuantity = maxAllowed;
        input.value = maxAllowed;
    }

    changeProductCount(productId, oldQuantity - newQuantity);

    cartItem.quantity = newQuantity;

    changeCartItemCountAndTotalPrice();
});

cartList.addEventListener('click', function (event) {
    if (!event.target.classList.contains('cart-remove-product-btn'))
        return;

    var removeBtn = event.target;

    var cartItemCard = removeBtn.closest('.cart-item-card');
    var productId = Number(cartItemCard.dataset.cartProductId);

    var cartItem = cartItems.find(ci => ci.productId == productId);
    cartItems = cartItems.filter(ci => ci.productId !== productId);

    changeProductCount(productId, cartItem.quantity);
    renderCart();
});

productList.addEventListener('click', async function(event) {
    if(!event.target.classList.contains('product-delete-btn'))
        return;

    var productDeleteBtn = event.target;

    var response = await fetch("/Shop/DeleteProduct", {
        method: 'post',
        headers: {'Content-Type' : 'application/json'},
        body: JSON.stringify(productDeleteBtn.dataset.productId)
    });

    var productsData = await response.json();

    products = productsData;

    renderProductList();
});