/* Chatbot Javascript - Premium E-commerce Assistant */

document.addEventListener("DOMContentLoaded", () => {
    const toggleBtn = document.querySelector(".chatbot-toggle");
    const closeBtn = document.querySelector(".chatbot-close");
    const chatWindow = document.querySelector(".chatbot-window");
    const msgContainer = document.querySelector(".chatbot-messages");
    const chatInput = document.querySelector(".chatbot-input");
    const sendBtn = document.querySelector(".chatbot-send-btn");

    let chatState = sessionStorage.getItem("chatbot_state") || "";

    // Toggle Chat Window
    if (toggleBtn && chatWindow) {
        toggleBtn.addEventListener("click", () => {
            chatWindow.classList.toggle("active");
            if (chatWindow.classList.contains("active")) {
                chatInput.focus();
                scrollToBottom();
            }
        });
    }

    if (closeBtn && chatWindow) {
        closeBtn.addEventListener("click", () => {
            chatWindow.classList.remove("active");
        });
    }

    // Handle Input Events
    if (chatInput) {
        chatInput.addEventListener("keypress", (e) => {
            if (e.key === "Enter") {
                handleSend();
            }
        });
    }

    if (sendBtn) {
        sendBtn.addEventListener("click", handleSend);
    }

    // Initialize Chat
    initChat();

    function initChat() {
        const cachedHistory = sessionStorage.getItem("chatbot_history");
        if (cachedHistory) {
            msgContainer.innerHTML = cachedHistory;
            // Re-attach event listeners to any option buttons loaded from cache
            attachOptionListeners();
            scrollToBottom();
        } else {
            // First time load: Trigger default greeting from backend
            sendToBot("");
        }
    }

    // Scroll to bottom
    function scrollToBottom() {
        if (msgContainer) {
            setTimeout(() => {
                msgContainer.scrollTop = msgContainer.scrollHeight;
            }, 50);
        }
    }

    // Add Message to Chat UI
    function appendMessage(sender, text, products = null, options = null) {
        if (!msgContainer) return;

        // Remove any existing temporary option button blocks before adding new messages
        const existingOptions = msgContainer.querySelector(".chatbot-options-container");
        if (existingOptions) {
            existingOptions.remove();
        }

        // Create Message Bubble
        const msgDiv = document.createElement("div");
        msgDiv.className = `chatbot-msg ${sender}`;
        
        // Handle line breaks in bot messages
        if (sender === "bot") {
            msgDiv.innerHTML = text.replace(/\n/g, "<br>");
        } else {
            msgDiv.textContent = text;
        }

        msgContainer.appendChild(msgDiv);

        // Render Products if returned
        if (products && products.length > 0) {
            const productsGrid = document.createElement("div");
            productsGrid.className = "chatbot-products-grid";

            products.forEach(p => {
                const card = document.createElement("a");
                card.className = "chatbot-product-card";
                card.href = `/Customer/Home/Product/${p.id}`;

                // Fallback image if main image is empty
                const imgSrc = p.mainImg ? p.mainImg : "/images/placeholder.jpg";

                card.innerHTML = `
                    <img src="${imgSrc}" class="chatbot-product-img" alt="${p.name}">
                    <div class="chatbot-product-info">
                        <p class="chatbot-product-name">${p.name}</p>
                        <span class="chatbot-product-price">${p.price.toLocaleString('en-US', { style: 'currency', currency: 'EGP' })}</span>
                    </div>
                    <i class='bx bx-chevron-left chatbot-product-arrow'></i>
                `;
                productsGrid.appendChild(card);
            });

            msgContainer.appendChild(productsGrid);
        }

        // Render Quick Reply Option Buttons
        if (options && options.length > 0) {
            const optionsDiv = document.createElement("div");
            optionsDiv.className = "chatbot-options-container";

            options.forEach(opt => {
                const optBtn = document.createElement("button");
                optBtn.className = "chatbot-option-btn";
                optBtn.textContent = opt;
                optionsDiv.appendChild(optBtn);
            });

            msgContainer.appendChild(optionsDiv);
            attachOptionListeners();
        }

        scrollToBottom();

        // Save entire innerHTML to sessionStorage
        sessionStorage.setItem("chatbot_history", msgContainer.innerHTML);
    }

    // Attach listeners to option buttons
    function attachOptionListeners() {
        const optionBtns = document.querySelectorAll(".chatbot-option-btn");
        optionBtns.forEach(btn => {
            // Remove previous event listener to avoid double binding
            btn.replaceWith(btn.cloneNode(true));
        });

        // Query again and attach fresh listeners
        document.querySelectorAll(".chatbot-option-btn").forEach(btn => {
            btn.addEventListener("click", () => {
                const choice = btn.textContent;
                appendMessage("user", choice);
                sendToBot(choice);
            });
        });
    }

    // Send Message Logic
    function handleSend() {
        if (!chatInput) return;
        const text = chatInput.value.trim();
        if (!text) return;

        chatInput.value = "";
        appendMessage("user", text);
        sendToBot(text);
    }

    // Send payload to backend API
    function sendToBot(messageText) {
        const payload = {
            Message: messageText,
            State: chatState
        };

        // Add a temporary typing visual
        const typingIndicator = document.createElement("div");
        typingIndicator.className = "chatbot-msg bot typing-msg";
        typingIndicator.textContent = "جاري الرد...";
        typingIndicator.style.opacity = "0.7";
        msgContainer.appendChild(typingIndicator);
        scrollToBottom();

        fetch("/Customer/Chatbot/GetResponse", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(payload)
        })
        .then(response => {
            if (!response.ok) {
                throw new Error("API call failed");
            }
            return response.json();
        })
        .then(data => {
            // Remove typing indicator
            const indicator = msgContainer.querySelector(".typing-msg");
            if (indicator) indicator.remove();

            // Update local state
            chatState = data.nextState;
            sessionStorage.setItem("chatbot_state", chatState);

            // Display Bot reply
            appendMessage("bot", data.message, data.products, data.options);
        })
        .catch(err => {
            console.error("Chatbot Error:", err);
            const indicator = msgContainer.querySelector(".typing-msg");
            if (indicator) indicator.remove();
            
            appendMessage("bot", "عذراً، حدث خطأ في الاتصال بالسيرفر. يرجى المحاولة مرة أخرى لاحقاً.");
        });
    }
});
