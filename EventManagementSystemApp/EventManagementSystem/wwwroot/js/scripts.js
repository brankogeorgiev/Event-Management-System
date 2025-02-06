/*!
* Start Bootstrap - Freelancer v7.0.7 (https://startbootstrap.com/theme/freelancer)
* Copyright 2013-2023 Start Bootstrap
* Licensed under MIT (https://github.com/StartBootstrap/startbootstrap-freelancer/blob/master/LICENSE)
*/
//
// Scripts
// 

window.addEventListener('DOMContentLoaded', event => {

    // Navbar shrink function
    var navbarShrink = function () {
        const navbarCollapsible = document.body.querySelector('#mainNav');
        if (!navbarCollapsible) {
            return;
        }
        if (window.scrollY === 0) {
            navbarCollapsible.classList.remove('navbar-shrink')
        } else {
            navbarCollapsible.classList.add('navbar-shrink')
        }

    };

    // Shrink the navbar 
    navbarShrink();

    // Shrink the navbar when page is scrolled
    document.addEventListener('scroll', navbarShrink);

    // Activate Bootstrap scrollspy on the main nav element
    const mainNav = document.body.querySelector('#mainNav');
    if (mainNav) {
        new bootstrap.ScrollSpy(document.body, {
            target: '#mainNav',
            rootMargin: '0px 0px -40%',
        });
    };

    // Collapse responsive navbar when toggler is visible
    const navbarToggler = document.body.querySelector('.navbar-toggler');
    const responsiveNavItems = [].slice.call(
        document.querySelectorAll('#navbarResponsive .nav-link')
    );
    responsiveNavItems.map(function (responsiveNavItem) {
        responsiveNavItem.addEventListener('click', () => {
            if (window.getComputedStyle(navbarToggler).display !== 'none') {
                navbarToggler.click();
            }
        });
    });

});
document.addEventListener("DOMContentLoaded", function () {
    const cartCountElement = document.getElementById("cart-count");

    fetch('/ShoppingCarts/GetCartItemCount') // Adjust the API endpoint accordingly
        .then(response => response.json())
        .then(data => {
            console.log("Cart Count Response:", data); // Debugging: Check if data is correct

            if (data.count > 0) {
                cartCountElement.textContent = data.count;
                cartCountElement.style.display = "inline-block"; // Ensure it's visible
            } else {
                cartCountElement.style.display = "none"; // Hide if cart is empty
            }
        })
        .catch(error => {
            console.error("Error fetching cart count:", error);
            cartCountElement.style.display = "none"; // Hide in case of an error
        });
});

function updateCartCount() {
    fetch('/ShoppingCarts/GetCartItemCount')
        .then(response => response.json())
        .then(data => {
            const cartCountElement = document.getElementById("cart-count");

            if (data.count > 0) {
                cartCountElement.textContent = data.count;
                cartCountElement.style.display = "inline-block";
            } else {
                cartCountElement.style.display = "none";
            }
        })
        .catch(error => {
            console.error("Error fetching cart count:", error);
        });
}

// Run on page load and every 10 seconds
document.addEventListener("DOMContentLoaded", updateCartCount);
setInterval(updateCartCount, 2000);
