let debounceTimeout;
let searchInput;
let searchResult;

document.addEventListener("DOMContentLoaded", function () {
    searchInput = document.querySelector("#searchInput");
    searchResult = document.querySelector("#searchResult");

    searchInput.addEventListener("keyup", searchKeyUp);
    searchInput.addEventListener("blur", searchBlur);
    searchInput.addEventListener("focus", search);
});

function searchKeyUp() {
    const { value } = searchInput;
    const { value: oldValue } = searchInput.dataset;

    if (value === oldValue) return;

    searchInput.dataset.value = value;
    clearTimeout(debounceTimeout);
    debounceTimeout = setTimeout(search, 300);
}

function searchBlur() {
    searchResult.innerHTML = '';
}

async function search() {
    const { value } = searchInput;
    searchResult.innerHTML = '';

    if (value.length === 0) return;

    try {
        const response = await fetch(`/Posts/Search?keyword=${value}`);
        if (!response.ok) {
            console.error('HTTP error', response.status);
        } else {
            const posts = await response.json();
            for (let post of posts) {
                searchResult.appendChild(createPost(post));
            }
        }
    } catch (error) {
        console.error('Fetch error:', error);
    }
}

function createPost(post) {
    const li = document.createElement("li");
    const heading = document.createElement("h5");
    const text = document.createElement("div");

    heading.textContent = post.title;
    text.textContent = post.description;

    heading.classList.add("m0");
    li.classList.add("mb12");

    li.appendChild(heading);
    li.appendChild(text);

    return li;
}
