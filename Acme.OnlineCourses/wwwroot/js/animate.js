document.addEventListener('DOMContentLoaded', function () {

    // Initial animation for elements in viewport
    let elementsInView = document.querySelectorAll('.animate');

    // Immediately activate animations for elements above the fold
    animateElementsInView();

    // Set up scroll observer for elements that come into view while scrolling
    const observer = new IntersectionObserver((entries, observer) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.classList.add('active');
                // Once the animation is triggered, no need to observe anymore
                observer.unobserve(entry.target);
            }
        });
    }, {
        root: null,
        rootMargin: '0px',
        threshold: 0.1
    });

    // Observe all animation elements
    document.querySelectorAll('.animate').forEach(element => {
        observer.observe(element);
    });

    // Function to immediately animate elements in the initial viewport
    function animateElementsInView() {
        const viewportHeight = window.innerHeight;

        elementsInView.forEach(element => {
            const rect = element.getBoundingClientRect();
            // If element is in the initial viewport
            if (rect.top >= 0 && rect.top <= viewportHeight) {
                // For elements at the top, animate immediately
                if (rect.top < viewportHeight * 0.5) {
                    element.classList.add('active');
                }
                // For elements lower in the viewport, add a slight delay
                else {
                    setTimeout(() => {
                        element.classList.add('active');
                    }, 300);
                }
            }
        });
    }

    // Add float animations to elements with float-animation class
    const floatElements = document.querySelectorAll('.float-animation');
    floatElements.forEach(element => {
        element.style.animation = 'float 4s ease-in-out infinite';
    });
});