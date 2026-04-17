const translations = {
    en: {
        "home": "Home",
        "hot-deals": "Hot Deals",
        "categories": "Categories",
        "laptops": "Laptops",
        "smartphones": "Smartphones",
        "cameras": "Cameras",
        "accessories": "Accessories",
        "my-account": "My Account",
        "admin-panel": "Admin Panel",
        "search-btn": "Search",
        "search-placeholder": "Search here"
    },
    ar: {
        "home": "الرئيسية",
        "hot-deals": "عروض مميزة",
        "categories": "الفئات",
        "laptops": "لابتوبات",
        "smartphones": "هواتف ذكية",
        "cameras": "كاميرات",
        "accessories": "إكسسوارات",
        "my-account": "حسابي",
        "admin-panel": "لوحة التحكم",
        "search-btn": "بحث",
        "search-placeholder": "ابحث هنا"
    }
};

function setLanguage(lang) {
    localStorage.setItem('lang', lang);
    document.documentElement.lang = lang;
    
    if (lang === 'ar') {
        document.documentElement.dir = 'rtl';
    } else {
        document.documentElement.dir = 'ltr';
    }

    const elements = document.querySelectorAll('[data-i18n]');
    elements.forEach(el => {
        const key = el.getAttribute('data-i18n');
        if (translations[lang] && translations[lang][key]) {
            if (el.tagName === 'INPUT' && (el.type === 'text' || el.type === 'search')) {
                el.placeholder = translations[lang][key];
            } else {
                el.innerText = translations[lang][key];
            }
        }
    });
}

function toggleLanguage() {
    let currentLang = localStorage.getItem('lang') || 'en';
    let newLang = currentLang === 'en' ? 'ar' : 'en';
    setLanguage(newLang);
    return newLang;
}

document.addEventListener('DOMContentLoaded', () => {
    let lang = localStorage.getItem('lang') || 'en';
    setLanguage(lang);
    
    let btn = document.getElementById('lang-toggle-btn');
    if(btn) {
        btn.innerText = lang === 'ar' ? 'English' : 'عربي';
        btn.addEventListener('click', (e) => {
            e.preventDefault();
            let newLang = toggleLanguage();
            btn.innerText = newLang === 'ar' ? 'English' : 'عربي';
        });
    }
});
